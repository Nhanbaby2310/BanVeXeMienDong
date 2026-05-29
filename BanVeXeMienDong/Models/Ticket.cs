namespace BanVeXeMienDong.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string TenKhachHang { get; set; }

        public string SoDienThoai { get; set; }

        public string DiemDi { get; set; }

        public string DiemDen { get; set; }

        public DateTime NgayDi { get; set; }

        public string SoGhe { get; set; }

        public decimal GiaVe { get; set; }

        public DateTime NgayDat { get; set; } = DateTime.Now;

        public BusClass HangXe { get; set; } = BusClass.Standard;
    }
}