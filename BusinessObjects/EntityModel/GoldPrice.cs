using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.EntityModel
{
    [Table("GoldPrice")]
    public class GoldPrice : EntityBase
    {
        public int GoldTypeId { get; set; }
        [Required]
        public decimal BuyPrice { get; set; }
        [Required]
        public decimal SellPrice { get; set; }
        [Required]
        public DateTime RecordedAt { get; set; }
        public virtual GoldType GoldType { get; set; } = null!;
    }
}