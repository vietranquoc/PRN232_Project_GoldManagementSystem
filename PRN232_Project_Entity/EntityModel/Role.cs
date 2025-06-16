using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.EntityModel
{
    public class Role : EntityBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
