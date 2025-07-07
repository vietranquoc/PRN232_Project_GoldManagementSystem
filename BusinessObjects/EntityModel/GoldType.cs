using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.EntityModel
{
    [Table("GoldType")]
    public class GoldType : EntityBase
    {
        [Required]
        public string Name { get; set; } = null!; // VD: "XAU/USD"  
        public int? Karat { get; set; } // VD: 24, 18 (nullable vì Spot không có karat)
        public string PriceType { get; set; } // VD: "Spot", "Gram"
        public string? Description { get; set; }
        public virtual ICollection<GoldPrice> GoldPrices { get; set; } = new List<GoldPrice>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
 