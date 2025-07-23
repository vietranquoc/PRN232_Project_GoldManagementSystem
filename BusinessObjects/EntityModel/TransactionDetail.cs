using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.EntityModel
{
    [Table("TransactionDetail")]
    public class TransactionDetail : EntityBase
    {
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual Transaction Transaction { get; set; }
        public virtual Product Product { get; set; }
    }
} 