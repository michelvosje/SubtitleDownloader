using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenSubtitles.Models
{
    [JsonObject]
    public class ErrorResponse
    {
        [JsonProperty("errors")]
        public List<string> Errors { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
