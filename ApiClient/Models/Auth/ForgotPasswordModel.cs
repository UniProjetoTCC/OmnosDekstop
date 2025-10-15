using System.ComponentModel.DataAnnotations;

namespace Omnos.Desktop.ApiClient.Models.Auth
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
