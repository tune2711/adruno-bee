using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace myapp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int CashierId { get; set; }
        public string CashierEmail { get; set; } = "";

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? BankingTransactionId { get; set; } // Added for banking integration

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
