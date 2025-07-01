using BusinessObjects.EntityBase;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.EntityModel
{
    public class EntityBase : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
