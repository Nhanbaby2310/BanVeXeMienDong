using Microsoft.AspNetCore.Mvc;
using BanVeXeMienDong.Services;

namespace BanVeXeMienDong.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartApiController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartApiController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("count")]
        public IActionResult GetCartCount()
        {
            var count = _cartService.GetCartCount();
            return Ok(new { count = count });
        }
    }
}
