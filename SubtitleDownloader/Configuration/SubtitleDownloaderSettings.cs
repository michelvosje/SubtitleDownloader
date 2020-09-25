namespace SubtitleDownloader.Configuration
{
    public class SubtitleDownloaderSettings
    {
        public OpenSubtitlesSettings OpenSubtitles { get; set; }

        public class OpenSubtitlesSettings
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string LanguageFilter { get; set; }
        }
    }
}
