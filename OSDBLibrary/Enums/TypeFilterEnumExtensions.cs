using System;

namespace OpenSubtitles.Enums
{
    public static class TypeFilterEnumExtensions
    {
        public static string Format(this TypeFilterEnum typeFilter)
        {
            switch (typeFilter)
            {
                case TypeFilterEnum.Movie:
                    return "movie";
                case TypeFilterEnum.Episode:
                    return "episode";
                case TypeFilterEnum.All:
                    return "all";
                default:
                    throw new ArgumentException(nameof(typeFilter));
            }
        }
    }
}
