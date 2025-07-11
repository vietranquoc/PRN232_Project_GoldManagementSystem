using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.EntityModel
{
    public class Category : EntityBase
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
} 