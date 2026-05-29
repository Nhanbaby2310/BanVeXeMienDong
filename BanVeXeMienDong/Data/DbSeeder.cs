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

            // === TUYẾN MIỀN NAM ===
            AddTripsForRoute("HCM", "Cần Thơ", 150000m, 200000m, 15);
            AddTripsForRoute("HCM", "Vũng Tàu", 120000m, 160000m, 20);
            AddTripsForRoute("HCM", "Đà Lạt", 250000m, 320000m, 12);
            AddTripsForRoute("HCM", "Nha Trang", 280000m, 350000m, 10);
            AddTripsForRoute("HCM", "Phan Thiết", 180000m, 220000m, 15);
            AddTripsForRoute("HCM", "Long An", 80000m, 100000m, 20);
            AddTripsForRoute("HCM", "Bến Tre", 130000m, 170000m, 12);
            AddTripsForRoute("HCM", "Tiền Giang", 100000m, 130000m, 15);
            AddTripsForRoute("HCM", "Đồng Nai", 60000m, 80000m, 25);
            AddTripsForRoute("HCM", "Bình Dương", 40000m, 60000m, 30);
            AddTripsForRoute("HCM", "Tây Ninh", 120000m, 150000m, 12);
            AddTripsForRoute("HCM", "An Giang", 200000m, 250000m, 10);
            AddTripsForRoute("HCM", "Kiên Giang", 250000m, 300000m, 8);
            AddTripsForRoute("HCM", "Cà Mau", 300000m, 380000m, 8);
            AddTripsForRoute("HCM", "Buôn Ma Thuột", 320000m, 400000m, 8);
            AddTripsForRoute("HCM", "Quy Nhơn", 350000m, 420000m, 8);
            AddTripsForRoute("Cần Thơ", "Sóc Trăng", 80000m, 100000m, 12);
            AddTripsForRoute("Cần Thơ", "Cà Mau", 150000m, 200000m, 10);
            AddTripsForRoute("Đà Lạt", "Nha Trang", 130000m, 170000m, 10);
            AddTripsForRoute("Vũng Tàu", "Phan Thiết", 150000m, 200000m, 8);

            // === TUYẾN MIỀN TRUNG ===
            AddTripsForRoute("Đà Nẵng", "HCM", 450000m, 550000m, 12);
            AddTripsForRoute("Đà Nẵng", "Huế", 100000m, 130000m, 20);
            AddTripsForRoute("Đà Nẵng", "Quảng Ngãi", 120000m, 150000m, 15);
            AddTripsForRoute("Đà Nẵng", "Quy Nhơn", 180000m, 220000m, 10);
            AddTripsForRoute("Đà Nẵng", "Hà Nội", 450000m, 550000m, 8);
            AddTripsForRoute("Huế", "HCM", 500000m, 600000m, 8);
            AddTripsForRoute("Huế", "Hà Nội", 380000m, 450000m, 8);
            AddTripsForRoute("Nha Trang", "Đà Nẵng", 300000m, 380000m, 8);
            AddTripsForRoute("Nha Trang", "Quy Nhơn", 180000m, 220000m, 8);
            AddTripsForRoute("Quy Nhơn", "HCM", 380000m, 450000m, 8);

            // === TUYẾN MIỀN BẮC ===
            AddTripsForRoute("Hà Nội", "Hải Phòng", 100000m, 130000m, 20);
            AddTripsForRoute("Hà Nội", "Quảng Ninh", 150000m, 200000m, 15);
            AddTripsForRoute("Hà Nội", "Ninh Bình", 100000m, 130000m, 15);
            AddTripsForRoute("Hà Nội", "Thanh Hóa", 150000m, 200000m, 12);
            AddTripsForRoute("Hà Nội", "Nghệ An", 220000m, 280000m, 10);
            AddTripsForRoute("Hà Nội", "Đà Nẵng", 380000m, 450000m, 8);
            AddTripsForRoute("Hà Nội", "HCM", 500000m, 600000m, 6);
            AddTripsForRoute("Hà Nội", "Sapa", 280000m, 350000m, 10);
            AddTripsForRoute("Hà Nội", "Lào Cai", 250000m, 320000m, 10);
            AddTripsForRoute("Hà Nội", "Bắc Ninh", 50000m, 70000m, 25);
            AddTripsForRoute("Hà Nội", "Bắc Giang", 70000m, 90000m, 20);
            AddTripsForRoute("Hà Nội", "Thái Nguyên", 80000m, 110000m, 15);
            AddTripsForRoute("Hà Nội", "Nam Định", 100000m, 130000m, 15);
            AddTripsForRoute("Hà Nội", "Hà Giang", 300000m, 380000m, 8);
            AddTripsForRoute("Hà Nội", "Điện Biên", 350000m, 420000m, 6);
            AddTripsForRoute("Hải Phòng", "Quảng Ninh", 80000m, 100000m, 15);

            context.Tickets.AddRange(sampleTickets);
            context.SaveChanges();
        }
    }
}
