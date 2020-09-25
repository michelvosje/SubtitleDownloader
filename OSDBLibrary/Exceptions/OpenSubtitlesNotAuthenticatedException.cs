using System;
using System.Runtime.Serialization;

namespace OpenSubtitles.Exceptions
{
    public class OpenSubtitlesNotAuthenticatedException : OpenSubtitlesException
    {
        public OpenSubtitlesNotAuthenticatedException()
        {
        }

        public OpenSubtitlesNotAuthenticatedException(string message)
            : base(message)
        {
        }

        public OpenSubtitlesNotAuthenticatedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected OpenSubtitlesNotAuthenticatedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
