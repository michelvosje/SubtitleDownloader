using System;
using System.Runtime.Serialization;

namespace OpenSubtitles.Exceptions
{
    public class OpenSubtitlesException : Exception
    {
        public OpenSubtitlesException()
        {
        }

        public OpenSubtitlesException(string message)
            : base(message)
        {
        }

        public OpenSubtitlesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected OpenSubtitlesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
