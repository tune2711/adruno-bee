
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myapp.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public string? Bank { get; set; }

        public DateTime Timestamp { get; set; }
        
        public string? UserName { get; set; }

        public string? TransactionId { get; set; }
    }
}
