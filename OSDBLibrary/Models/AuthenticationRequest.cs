using Newtonsoft.Json;

namespace OpenSubtitles.Models
{
    [JsonObject]
    public class AuthenticationRequest
    {
        [JsonRequired]
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonRequired]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
