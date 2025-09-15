
using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
