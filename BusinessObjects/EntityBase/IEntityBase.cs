using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.EntityBase
{
    public interface IEntityBase
    {
        [Key] int Id { get; set; }
        [Required] bool IsActive { get; set; }
        [Required] DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        [Required] int CreatedBy { get; set; }
        int? UpdatedBy { get; set; }
    }
}
