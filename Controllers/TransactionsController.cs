
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
    [Route("api")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly Random _random = new Random();

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GET")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.OrderByDescending(t => t.Timestamp).ToListAsync();
        }

        [HttpPost("POST")]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            var datePart = DateTime.Now.ToString("ddMMyyyy");
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomPart = new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            transaction.TransactionId = $"{datePart}{randomPart}";
            transaction.Timestamp = DateTime.Now;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactions", new { id = transaction.Id }, transaction);
        }

        [HttpGet("GET/{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpGet("GET/id/{transactionId}")]
        public async Task<ActionResult<Transaction>> GetTransactionByTransactionId(string transactionId)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }
    }
}
