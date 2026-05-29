using Microsoft.AspNetCore.Mvc;
using BanVeXeMienDong.Services;
using BanVeXeMienDong.Attributes;
using BanVeXeMienDong.Data;
using BanVeXeMienDong.Models;
using System.Text.Json;

namespace BanVeXeMienDong.Controllers
{
    [Authorize("Admin", "User")]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly AppDbContext _context;

        public CheckoutController(ICartService cartService, AppDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        // 💳 Trang thanh toán
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            if (cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn trống!";
                return RedirectToAction("Index", "Cart");
            }

            var total = _cartService.GetTotal();
            ViewBag.Total = total;
            ViewBag.Cart = cart;

            return View();
        }

        // ✅ Xử lý thanh toán
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessPayment(string paymentMethod, string phoneNumber, string email, string notes)
        {
            var cart = _cartService.GetCart();
            if (cart.Count == 0)
            {
                return BadRequest("Giỏ hàng trống!");
            }

            var userId = HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var total = _cartService.GetTotal();
            var orderCode = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss") + userId;

            var order = new Order
            {
                UserId = userId.Value,
                OrderCode = orderCode,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                Status = "Confirmed",
                ItemsJson = JsonSerializer.Serialize(cart),
                PaymentMethod = paymentMethod,
                PhoneNumber = phoneNumber,
                Email = email
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            _cartService.ClearCart();

            TempData["Success"] = "✅ Thanh toán thành công!";
            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }

        // 📋 Xác nhận đơn hàng
        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            var items = JsonSerializer.Deserialize<List<CartItem>>(order.ItemsJson);
            ViewBag.Items = items;

            return View(order);
        }

        // 📜 Danh sách đơn hàng của user
        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var orders = _context.Orders
                .Where(o => o.UserId == userId.Value)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // 📄 Chi tiết đơn hàng
        public IActionResult OrderDetail(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("userId");
            if (order.UserId != userId)
            {
                return Unauthorized();
            }

            var items = JsonSerializer.Deserialize<List<CartItem>>(order.ItemsJson);
            ViewBag.Items = items;

            return View(order);
        }

        // ❌ Hủy đơn hàng (POST + AntiForgery - chống CSRF)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("userId");
            if (order.UserId != userId)
            {
                return Unauthorized();
            }

            if (order.Status == "Confirmed")
            {
                order.Status = "Cancelled";
                _context.SaveChanges();
                TempData["Success"] = "✅ Đơn hàng đã được hủy!";
            }

            return RedirectToAction("MyOrders");
        }
    }
}
