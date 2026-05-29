namespace BanVeXeMienDong.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderCode { get; set; } = "";
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
        public string ItemsJson { get; set; } = "";  // JSON string của các item
        public string PaymentMethod { get; set; } = "Cash"; // Cash, Card, etc.
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
