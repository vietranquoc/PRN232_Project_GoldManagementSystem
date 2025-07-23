using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects.EntityModel
{
    [Table("CartItem")]
    public class CartItem : EntityBase
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
} 