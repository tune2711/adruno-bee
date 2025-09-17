
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myapp.Models
{
    public class Transaction
    {
    [System.Text.Json.Serialization.JsonIgnore]
    public int UserId { get; set; }
    [Key]
    public string? TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public string? Bank { get; set; }

        public DateTime Timestamp { get; set; }
        
        public string? UserName { get; set; }

    }
}
