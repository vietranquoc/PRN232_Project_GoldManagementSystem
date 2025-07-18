using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }
    }
} 