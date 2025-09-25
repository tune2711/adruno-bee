using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myapp.Data;
using myapp.Models;
using System.Security.Claims;
using System.Text;

namespace myapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders/{id}/receipt
        [HttpGet("{id}/receipt")]
        [AllowAnonymous] // Or use [Authorize] if you want to protect it
        public async Task<IActionResult> GetReceipt(int id)
        {
            var order = await _context.Orders
                                      .Include(o => o.OrderItems)
                                      .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            var receiptHtml = GenerateReceiptHtml(order);
            // Return content with UTF-8 encoding specified
            return Content(receiptHtml, "text/html", System.Text.Encoding.UTF8);
        }

        // GET: api/Orders/receipt-by-transaction/{transactionId}
        [HttpGet("receipt-by-transaction/{transactionId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReceiptByTransactionId(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                return BadRequest(new { message = "Transaction ID cannot be empty." });
            }

            var order = await _context.Orders
                                      .Include(o => o.OrderItems)
                                      .FirstOrDefaultAsync(o => o.BankingTransactionId == transactionId);

            if (order == null)
            {
                return NotFound(new { message = $"Order with transaction ID '{transactionId}' not found." });
            }

            var receiptHtml = GenerateReceiptHtml(order);
            return Content(receiptHtml, "text/html", System.Text.Encoding.UTF8);
        }


        // POST: api/Orders/receipt
        [HttpPost("receipt")]
        [Authorize(Roles = "staff,manager,admin")]
        public async Task<IActionResult> CreateReceipt([FromBody] Cart cart)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { message = "User email not found in token." });
            }

            var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (appUser == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    return BadRequest(new { message = $"Product with ID {item.ProductId} not found." });
                }

                var orderItem = new OrderItem
                {
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    Price = product.Price
                };
                orderItems.Add(orderItem);
                totalAmount += orderItem.Quantity * orderItem.Price;
            }

            var order = new Order
            {
                CashierId = appUser.Id,
                CashierEmail = appUser.Email ?? "",
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderItems = orderItems,
                BankingTransactionId = cart.BankingTransactionId // Save the transaction ID
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Instead of returning HTML directly, return the URL to view the receipt
            return CreatedAtAction(nameof(GetReceipt), new { id = order.Id }, new { receiptUrl = $"/api/Orders/{order.Id}/receipt" });
        }

        private string GenerateReceiptHtml(Order order)
        {
            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            // Add Meta tag for UTF-8
            sb.Append("<meta charset=\"UTF-8\">");
            sb.Append("<style>");
            sb.Append("body { font-family: sans-serif; margin: 20px; }");
            sb.Append(".receipt-container { border: 1px solid #ccc; padding: 20px; width: 300px; margin: auto; box-shadow: 0 0 10px rgba(0,0,0,0.1); }");
            sb.Append(".header { text-align: center; margin-bottom: 20px; }");
            sb.Append(".header img { width: 100px; margin-bottom: 10px; }");
            sb.Append("h2 { margin: 0; }");
            sb.Append("table { width: 100%; border-collapse: collapse; }");
            sb.Append("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            sb.Append(".footer { text-align: center; margin-top: 20px; font-size: 0.8em; }");
            sb.Append("</style></head>");
            sb.Append("<body>");
            sb.Append("<div class='receipt-container'>");
            sb.Append("<div class='header'>");
            sb.Append("<img src='https://i.imgur.com/2OfaDPE.png' alt='Nightfood Logo'>");
            sb.Append("<h2>Phiếu Thanh Toán</h2>");
            sb.Append("</div>");
            // sb.Append($"<p><strong>Mã đơn:</strong> {order.Id}</p>"); // Added Order ID
            sb.Append($"<p><strong>Thu ngân (user):</strong> {order.CashierEmail}</p>");
            sb.Append($"<p><strong>Ngày tạo:</strong> {order.OrderDate:dd/MM/yyyy HH:mm:ss}</p>");
            if (!string.IsNullOrEmpty(order.BankingTransactionId))
            {
                sb.Append($"<p><strong>Mã Đơn:</strong> {order.BankingTransactionId}</p>");
            }
            sb.Append("<hr>");
            sb.Append("<table>");
            sb.Append("<tr><th>Tên</th><th>SL</th><th>Tiền</th></tr>");

            foreach (var item in order.OrderItems)
            {
                sb.Append($"<tr><td>{item.ProductName}</td><td>{item.Quantity}</td><td>{item.Price:N0} đ</td></tr>");
            }

            sb.Append("</table>");
            sb.Append("<hr>");
            sb.Append($"<h3>Thành tiền: {order.TotalAmount:N0} đ</h3>");
            sb.Append("<div class='footer'>Phần mềm bởi Gemini</div>");
            sb.Append("</div>");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}
