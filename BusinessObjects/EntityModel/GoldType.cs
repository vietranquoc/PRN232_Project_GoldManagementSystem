using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.EntityModel
{
    [Table("GoldType")]
    public class GoldType : EntityBase
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public virtual ICollection<GoldPrice> GoldPrices { get; set; } = new List<GoldPrice>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
 