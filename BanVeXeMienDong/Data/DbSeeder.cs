using BanVeXeMienDong.Models;

namespace BanVeXeMienDong.Data
{
    /// <summary>
    /// Seed dữ liệu vé mẫu vào database khi chưa có dữ liệu
    /// </summary>
    public static class DbSeeder
    {
        public static void SeedTickets(AppDbContext context)
        {
            // Chỉ seed nếu bảng Tickets trống
            if (context.Tickets.Any())
                return;

            var now = DateTime.Now;
            var sampleTickets = new List<Ticket>();

            // Helper function to create multiple trips for a route
            void AddTripsForRoute(string diemDi, string diemDen, decimal giaBan, decimal giaCao, int numTrips = 15)
            {
                for (int dayOffset = 0; dayOffset < 7; dayOffset++)
                {
                    var baseDate = now.Date.AddDays(dayOffset);
                    var startHour = 5;
                    var hoursPerTrip = 24 / numTrips;

                    for (int i = 0; i < numTrips; i++)
                    {
                        var hour = startHour + (i * hoursPerTrip);
                        if (hour >= 24) hour -= 24;

                        var isStandard = i % 3 != 2;
                        var gia = isStandard ? giaBan : giaCao;
                        var hangXe = isStandard ? BusClass.Standard : BusClass.Premium;

                        sampleTickets.Add(new Ticket
                        {
                            TenKhachHang = "",
                            SoDienThoai = "",
                            DiemDi = diemDi,
                            DiemDen = diemDen,
                            NgayDi = baseDate.AddHours(hour),
                            SoGhe = $"{(char)('A' + (i % 10))}{(i % 40) + 1}",
                            GiaVe = gia,
                            HangXe = hangXe,
                            NgayDat = now
                        });
                    }
                }
            }

            // Tạo dữ liệu cho các tuyến phổ biến
            AddTripsForRoute("HCM", "Cần Thơ", 150000m, 180000m, 15);
            AddTripsForRoute("Hà Nội", "Hải Phòng", 100000m, 120000m, 15);
            AddTripsForRoute("Đà Nẵng", "HCM", 450000m, 500000m, 15);
            AddTripsForRoute("Hà Nội", "Đà Nẵng", 350000m, 400000m, 15);
            AddTripsForRoute("Hà Nội", "HCM", 400000m, 450000m, 12);
            AddTripsForRoute("HCM", "Đà Nẵng", 400000m, 450000m, 12);
            AddTripsForRoute("Hà Nội", "Quy Nhơn", 380000m, 420000m, 10);
            AddTripsForRoute("HCM", "Nha Trang", 250000m, 300000m, 10);
            AddTripsForRoute("Đà Nẵng", "Hải Phòng", 350000m, 400000m, 8);
            AddTripsForRoute("HCM", "Long An", 80000m, 100000m, 20);
            AddTripsForRoute("Hà Nội", "Bắc Ninh", 60000m, 80000m, 20);
            AddTripsForRoute("Cần Thơ", "Sóc Trăng", 120000m, 150000m, 12);

            context.Tickets.AddRange(sampleTickets);
            context.SaveChanges();
        }
    }
}
