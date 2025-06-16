using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.EntityModel
{
    public class User : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        [MaxLength(10)]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        [Required]
        [MaxLength(256)]
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
