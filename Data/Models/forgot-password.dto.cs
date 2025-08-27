using System.ComponentModel.DataAnnotations;

namespace BornOtomasyonApi.Models
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
