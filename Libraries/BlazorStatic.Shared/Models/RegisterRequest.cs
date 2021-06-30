using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorStatic.Shared.Services
{
    public class RegisterRequest
    {
        [Required]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Required]
        [JsonPropertyName("passwordConfirm")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
        public string PasswordConfirm { get; set; }
    }
}
