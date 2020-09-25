using Newtonsoft.Json;

namespace OpenSubtitles.Models
{
    [JsonObject]
    public class DownloadRequest
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }
    }
}
