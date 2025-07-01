namespace BusinessObjects.DTOs
{
    public class CreateTransactionDTO
    {
        public int GoldTypeId { get; set; }
        public string TransactionType { get; set; } = null!;
        public decimal Weight { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
