using Microsoft.AspNetCore.Mvc;
using BanVeXeMienDong.Services;
using BanVeXeMienDong.Attributes;
using BanVeXeMienDong.Models;

namespace BanVeXeMienDong.Controllers
{
    [Authorize("Admin", "User")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // 🛒 Xem giỏ hàng
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            var total = _cartService.GetTotal();
            
            ViewBag.Total = total;
            ViewBag.CartCount = _cartService.GetCartCount();
            
            return View(cart);
        }

        // ➕ Thêm vào giỏ hàng
        public IActionResult AddToCart(string ticketCode, string route, string seats, decimal price, string busClass, string departureTime)
        {
            var item = new CartItem
            {
                TicketCode = ticketCode,
                Route = route,
                Seats = seats,
                Price = price,
                Quantity = 1,
                BusClass = busClass,
                DepartureTime = departureTime
            };

            _cartService.AddToCart(item);

            TempData["Success"] = "✅ Thêm vào giỏ hàng thành công!";
            return RedirectToAction("Index");
        }

        // 🗑️ Xóa khỏi giỏ hàng
        public IActionResult RemoveFromCart(int itemId)
        {
            _cartService.RemoveFromCart(itemId);
            TempData["Success"] = "✅ Xóa khỏi giỏ hàng thành công!";
            return RedirectToAction("Index");
        }

        // 🗑️ Xóa toàn bộ giỏ hàng
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            TempData["Success"] = "✅ Giỏ hàng đã được xóa!";
            return RedirectToAction("Index");
        }

        // ✏️ Cập nhật số lượng
        [HttpPost]
        public IActionResult UpdateQuantity(int itemId, int quantity)
        {
            if (quantity < 1)
            {
                return BadRequest(new { message = "Số lượng phải >= 1" });
            }

            _cartService.UpdateCart(itemId, quantity);
            return Ok(new { message = "✅ Cập nhật số lượng thành công!" });
        }

        // ✏️ Cập nhật giá
        [HttpPost]
        public IActionResult UpdatePrice(int itemId, decimal price)
        {
            if (price < 0)
            {
                return BadRequest(new { message = "Giá không thể âm" });
            }

            var cart = _cartService.GetCart();
            var item = cart.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                item.Price = price;
                // Cập nhật item trong session trực tiếp
                var updatedCart = cart;
                SaveCartDirectly(updatedCart);
                return Ok(new { message = "✅ Cập nhật giá thành công!" });
            }

            return NotFound(new { message = "Không tìm thấy item" });
        }

        // Helper method để lưu giỏ hàng trực tiếp
        private void SaveCartDirectly(List<CartItem> cart)
        {
            var session = HttpContext?.Session;
            if (session != null)
            {
                var cartJson = System.Text.Json.JsonSerializer.Serialize(cart, new System.Text.Json.JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                });
                session.SetString("cart", cartJson);
            }
        }
    }
}