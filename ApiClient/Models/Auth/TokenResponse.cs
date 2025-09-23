using System;
using System.Text.Json.Serialization;

namespace Omnos.Desktop.ApiClient.Models.Auth
{
    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string AccessToken { get; set; } = string.Empty;
        
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;
        
        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }
    }
}
