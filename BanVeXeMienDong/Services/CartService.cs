using BanVeXeMienDong.Models;
using System.Text.Json;

namespace BanVeXeMienDong.Services
{
    public interface ICartService
    {
        void AddToCart(CartItem item);
        void RemoveFromCart(int itemId);
        void UpdateCart(int itemId, int quantity);
        List<CartItem> GetCart();
        void SaveCart(List<CartItem> cart);
        void ClearCart();
        decimal GetTotal();
        int GetCartCount();
    }

    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "cart";
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCart();
            // 👉 KHÔNG MERGE ITEMS - Mỗi ghế khác = item khác
            // Vì Quantity = số ghế, nên không cần merge
            if (cart.Count > 0)
            {
                item.Id = cart.Max(x => x.Id) + 1;
            }
            else
            {
                item.Id = 1;
            }
            cart.Add(item);

            SaveCartInternal(cart);
        }

        public void RemoveFromCart(int itemId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCartInternal(cart);
            }
        }

        public void UpdateCart(int itemId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                item.Quantity = quantity;
                SaveCartInternal(cart);
            }
        }

        public List<CartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
                return new List<CartItem>();

            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson, JsonOptions) ?? new List<CartItem>();
        }

        public void ClearCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Remove(CartSessionKey);
            }
        }

        public void SaveCart(List<CartItem> cart)
        {
            SaveCartInternal(cart);
        }

        public decimal GetTotal()
        {
            var cart = GetCart();
            // Price đã là tổng giá (pricePerSeat × số ghế), KHÔNG nhân thêm Quantity
            return cart.Sum(x => x.Price);
        }

        public int GetCartCount()
        {
            var cart = GetCart();
            return cart.Sum(x => x.Quantity);
        }

        private void SaveCartInternal(List<CartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var cartJson = JsonSerializer.Serialize(cart, JsonOptions);
                session.SetString(CartSessionKey, cartJson);
            }
        }
    }
}
