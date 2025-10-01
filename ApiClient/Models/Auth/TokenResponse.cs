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

        [JsonPropertyName("requiresTwoFactor")]
        public bool TwoFactorRequired { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;


    }
}
