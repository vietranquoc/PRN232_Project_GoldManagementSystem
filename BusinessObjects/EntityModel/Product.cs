using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.EntityModel
{
    public class Product : EntityBase
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("GoldType")]
        public int GoldTypeId { get; set; }
        public virtual GoldType GoldType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
} 