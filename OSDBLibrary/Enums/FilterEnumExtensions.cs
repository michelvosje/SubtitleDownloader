using System;

namespace OpenSubtitles.Enums
{
    public static class FilterEnumExtensions
    {
        public static string Format(this FilterEnum filter)
        {
            switch (filter) {
                case FilterEnum.Include:
                    return "include";
                case FilterEnum.Exclude:
                    return "exclude";
                case FilterEnum.Only:
                    return "only";
                default:
                    throw new ArgumentException(nameof(filter));
            }
        }
    }
}
