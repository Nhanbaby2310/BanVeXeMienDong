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

        // 🛒 Xem giỏ hàng (GET - an toàn, không thay đổi dữ liệu)
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            var total = _cartService.GetTotal();
            
            ViewBag.Total = total;
            ViewBag.CartCount = _cartService.GetCartCount();
            
            return View(cart);
        }

        // ➕ Thêm vào giỏ hàng (POST + AntiForgery)
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // 🗑️ Xóa khỏi giỏ hàng (POST + AntiForgery)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int itemId)
        {
            _cartService.RemoveFromCart(itemId);
            TempData["Success"] = "✅ Xóa khỏi giỏ hàng thành công!";
            return RedirectToAction("Index");
        }

        // 🗑️ Xóa toàn bộ giỏ hàng (POST + AntiForgery)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            TempData["Success"] = "✅ Giỏ hàng đã được xóa!";
            return RedirectToAction("Index");
        }

        // ✏️ Cập nhật số lượng (POST + AntiForgery)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int itemId, int quantity)
        {
            if (quantity < 1)
            {
                return BadRequest(new { message = "Số lượng phải >= 1" });
            }

            _cartService.UpdateCart(itemId, quantity);
            return Ok(new { message = "✅ Cập nhật số lượng thành công!" });
        }

        // ✏️ Cập nhật giá (POST + AntiForgery)
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                _cartService.ClearCart();
                foreach (var cartItem in cart)
                {
                    _cartService.AddToCart(cartItem);
                }
                return Ok(new { message = "✅ Cập nhật giá thành công!" });
            }

            return NotFound(new { message = "Không tìm thấy item" });
        }
    }
}
