using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class UpdateProfileDTO
    {
        [Required]
        public string FullName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
} 