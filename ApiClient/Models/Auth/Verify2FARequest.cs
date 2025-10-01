// ApiClient/Models/Auth/Verify2FARequest.cs

using System.Text.Json.Serialization;

namespace Omnos.Desktop.ApiClient.Models.Auth
{
    public class Verify2FARequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
    }
}