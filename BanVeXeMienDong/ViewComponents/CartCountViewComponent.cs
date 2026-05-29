using Microsoft.AspNetCore.Mvc;
using BanVeXeMienDong.Services;

namespace BanVeXeMienDong.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartCountViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IViewComponentResult Invoke()
        {
            var cartCount = _cartService.GetCartCount();
            return View(cartCount);
        }
    }
}
