using Newtonsoft.Json;

namespace OpenSubtitles.Models
{
    [JsonObject]
    public class AuthenticationResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
            
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }

        [JsonObject]
        public class UserModel
        {
            [JsonProperty("jti")]
            public string JTI { get; set; }
                
            [JsonProperty("allowed_downloads")]
            public int AllowedDownloads { get; set; }
                
            [JsonProperty("level")]
            public string Level { get; set; }
                
            [JsonProperty("user_id")]
            public string UserId { get; set; }
                
            [JsonProperty("ext_installed")]
            public bool ExtInstalled { get; set; }
                
            [JsonProperty("vip")]
            public bool VIP { get; set; }
        }
    }
}
