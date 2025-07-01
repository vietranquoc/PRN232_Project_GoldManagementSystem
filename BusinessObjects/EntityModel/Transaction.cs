using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.EntityModel
{
    [Table("Transaction")]
    public class Transaction : EntityBase
    {
        public int UserId { get; set; }
        public int GoldTypeId { get; set; }
        [Required]
        [MaxLength(10)]
        public string TransactionType { get; set; } = null!; // "BUY" or "SELL"
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = null!; // "COMPLETED" or "PENDING"
        public virtual User User { get; set; } = null!;
        public virtual GoldType GoldType { get; set; } = null!;
    }
}