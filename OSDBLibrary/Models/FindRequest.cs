using OpenSubtitles.Enums;

namespace OpenSubtitles.Models
{
    public class FindRequest
    {
        public string Id { get; set; }
        public string ImdbId { get; set; }
        public string TmdbId { get; set; }
        public TypeFilterEnum Type { get; set; }
        public string Query { get; set; }
        public string Languages { get; set; }
        public string MovieHash { get; set; }
        public string UserId { get; set; }
        public FilterEnum HearingImpaired { get; set; }
        public FilterEnum TrustedSources { get; set; }
        public FilterEnum MachineTranslated { get; set; }
        public FilterEnum AiTranslated { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
    }
}
