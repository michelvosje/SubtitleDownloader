using System;
using System.Runtime.Serialization;

namespace OpenSubtitles.Exceptions
{
    public class OpenSubtitlesAuthenticationException : OpenSubtitlesException
    {
        public OpenSubtitlesAuthenticationException()
        {
        }

        public OpenSubtitlesAuthenticationException(string message) : base(message)
        {
        }

        public OpenSubtitlesAuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OpenSubtitlesAuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
