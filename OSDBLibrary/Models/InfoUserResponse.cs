using Newtonsoft.Json;

namespace OpenSubtitles.Models
{
    [JsonObject]
    public class InfoUserResponse
    {
        [JsonProperty("data")]
        public DataModel Data { get; set; }

        [JsonObject]
        public class DataModel
        {
            [JsonProperty("allowed_downloads")]
            public int AllowedDownloads { get; set; }

            [JsonProperty("user_id")]
            public string UserId { get; set; }

            [JsonProperty("vip")]
            public bool VIP { get; set; }

            [JsonProperty("ext_installed")]
            public bool ExtInstalled { get; set; }

            [JsonProperty("level")]
            public string Level { get; set; }

            [JsonProperty("remaining_downloads")]
            public int RemainingDownloads { get; set; }
        }
    }
}
