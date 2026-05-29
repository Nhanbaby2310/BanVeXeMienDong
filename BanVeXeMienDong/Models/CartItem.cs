namespace BanVeXeMienDong.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string TicketCode { get; set; }  // Mã vé
        public string Route { get; set; }       // Tuyến đường
        public string Seats { get; set; }       // Ghế
        public decimal Price { get; set; }      // Giá
        public int Quantity { get; set; }       // Số lượng
        public string BusClass { get; set; }    // Loại xe
        public string DepartureTime { get; set; } // Giờ khởi hành
    }
}
