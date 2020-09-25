namespace SubtitleDownloader
{
    public class ProgramParameters
    {
        public bool ClearUserConfiguration { get; set; }
        public bool ConfigureUser { get; set; }
        public bool ConfigureLanguageFilter { get; set; }
        public bool DownloadSpecificSubtitles { get; internal set; }
        public bool DownloadSubtitles { get; set; }
        public bool ListSubtitles { get; set; }
        public bool ShowStatus { get; set; }
        public bool PrintHelp { get; set; }
        public string MovieFileName { get; set; }
        public string SubtitlesId { get; set; }
    }
}
