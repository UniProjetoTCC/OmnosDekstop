using System;
using System.Text.Json.Serialization;

namespace Omnos.Desktop.ApiClient.Models.Auth
{
    public class TokenRequest
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
