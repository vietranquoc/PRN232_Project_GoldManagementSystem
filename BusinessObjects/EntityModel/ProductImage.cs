using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.EntityModel
{
    public class ProductImage : EntityBase
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public bool IsMain { get; set; }
    }
} 