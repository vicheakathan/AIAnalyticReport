using Newtonsoft.Json;

namespace AnalyticsReport.Core
{
    public class ClientToken
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("grant_type")]
        public string? GrantType { get; set; }

        [JsonProperty("expired_in")]
        public long ExpiredIn { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        public ClientToken()
        {

        }
    }
}
