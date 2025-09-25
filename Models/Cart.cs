namespace myapp.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public string? BankingTransactionId { get; set; }
    }
}
