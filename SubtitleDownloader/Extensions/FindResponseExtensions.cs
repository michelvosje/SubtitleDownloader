using F23.StringSimilarity;
using OpenSubtitles.Models;
using System.Collections.Generic;
using System.Linq;

namespace SubtitleDownloader.Extensions
{
    public static class FindResponseExtensions
    {

        /// <summary>
        /// Filters unwanted subtitles.
        /// </summary>
        /// <param name="subtitles">Subtitles to filter.</param>
        /// <returns>Filtered subtitles.</returns>
        public static IEnumerable<FindResponse.DataModel> FilterSubtitles(
            this IEnumerable<FindResponse.DataModel> subtitles)
        {
            return subtitles.Where(s => s.Attributes.Files.Count == 1);
        }

        /// <summary>
        /// Order the subtitles by prioritization.
        /// </summary>
        /// <param name="subtitles">Subtitles to prioritize.</param>
        /// <param name="movieName">The name of the movie for which to download the subtitles.</param>
        /// <returns>Subtitles ordered by prioritization.</returns>
        public static IEnumerable<FindResponse.DataModel> OrderByPrioritization(
            this IEnumerable<FindResponse.DataModel> subtitles, string movieName)
        {
            var levenshtein = new Levenshtein();

            return subtitles
                .OrderByDescending(s => s.Attributes.MovieHashMatch)
                .ThenBy(s => levenshtein.Distance(s.Attributes.Release, movieName))
                .ThenByDescending(s => s.Attributes.FromTrusted)
                .ThenBy(s => s.Attributes.HearingImpaired)
                .ThenBy(s => s.Attributes.AiTranslated)
                .ThenBy(s => s.Attributes.MachineTranslated.GetValueOrDefault());
        }
    }
}
