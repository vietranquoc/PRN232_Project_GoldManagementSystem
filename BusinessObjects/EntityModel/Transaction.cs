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
        public decimal UnitPrice { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = null!; // "COMPLETED" or "PENDING"
        [Required]
        [MaxLength(100)]
        public string ReceiverName { get; set; } = null!;
        [Required]
        [MaxLength(20)]
        public string ReceiverPhone { get; set; } = null!;
        [MaxLength(100)]
        public string? ReceiverEmail { get; set; }
        [Required]
        [MaxLength(100)]
        public string Province { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string District { get; set; } = null!;
        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = null!;
        [MaxLength(500)]
        public string? Note { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual GoldType GoldType { get; set; } = null!;
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    }
}