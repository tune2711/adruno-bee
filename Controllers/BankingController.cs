using Microsoft.AspNetCore.Mvc;
using myapp.Data;
using myapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace myapp.Controllers
{
    [Route("api/Banking")]
    [ApiController]
    public class BankingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly Random _random = new Random();

        public BankingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET APIs
        [HttpGet("create")]
        public ActionResult<Transaction> GetCreateTemplate()
        {
            var datePart = DateTime.Now.ToString("ddMMyyyy");
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomPart = new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            var template = new Transaction
            {
                Amount = 0,
                Description = "",
                Bank = "",
                Timestamp = DateTime.Now,
                UserName = "",
                TransactionId = $"{datePart}{randomPart}"
            };
            _context.Transactions.Add(template);
            _context.SaveChanges();
            return Ok(template);
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.OrderByDescending(t => t.Timestamp).ToListAsync();
        }

        [HttpGet("get/id/{transactionId}")]
        public async Task<ActionResult<Transaction>> GetTransactionByTransactionId(string transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // POST API
        [HttpPost("post")]
        public async Task<ActionResult<Transaction>> UpdateTransaction(Transaction transaction)
        {
            var existing = await _context.Transactions.FindAsync(transaction.TransactionId);
            if (existing == null)
            {
                return NotFound();
            }

            // Chỉ cập nhật các trường cần thiết
            existing.Amount = transaction.Amount;
            existing.Description = transaction.Description;
            existing.Bank = transaction.Bank;
            existing.UserName = transaction.UserName;
            existing.Timestamp = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE API
        [HttpDelete("delete/{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(string transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
