using System;
using System.IO;

namespace OpenSubtitles.Utility
{
    /// <summary>
    /// Provides functionality to computate an Open Subtitles movie hash which is used to
    /// uniquely identity movies.
    /// </summary>
    public static class MovieHasher
    {
        /// <summary>
        /// Computes a movie hash by movie file.
        /// </summary>
        /// <param name="filename">Movie file.</param>
        /// <returns>Computated movie hash.</returns>
        public static string ComputeMovieHash(string filename)
        {
            using (Stream input = File.OpenRead(filename))
            {
                var hashBytes = ComputeMovieHash(input);

                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
        }

        /// <summary>
        /// Computates a movie hash by movie file stream data.
        /// </summary>
        /// <param name="input">Provides movie file stream data.</param>
        /// <returns>Computated movie hash.</returns>
        private static byte[] ComputeMovieHash(Stream input)
        {
            long lhash, streamsize;
            streamsize = input.Length;
            lhash = streamsize;

            long i = 0;
            byte[] buffer = new byte[sizeof(long)];
            while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
            {
                i++;
                lhash += BitConverter.ToInt64(buffer, 0);
            }

            input.Position = Math.Max(0, streamsize - 65536);
            i = 0;
            while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
            {
                i++;
                lhash += BitConverter.ToInt64(buffer, 0);
            }
            input.Close();
            byte[] result = BitConverter.GetBytes(lhash);
            Array.Reverse(result);
            return result;
        }
    }
}
