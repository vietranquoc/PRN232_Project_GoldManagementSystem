using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class CreateEmployeeDTO
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
} 