using System;
using System.IO;

namespace SubtitleDownloader.Utility
{
    public static class SubtitleDownloaderSettingsLocator
    {
        public static string GetUserSettingsDirectory()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SubtitleDownloader"
            );
        }

        public static string GetUserSettingsFileName()
        {
            return Path.Combine(GetUserSettingsDirectory(), "settings.json");
        }
    }
}
