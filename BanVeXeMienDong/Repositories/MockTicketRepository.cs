using BanVeXeMienDong.Models;

namespace BanVeXeMienDong.Repositories
{
    public class MockTicketRepository : ITicketRepository
    {
        // Static list to persist data during application lifetime
        private static readonly List<Ticket> tickets = new List<Ticket>();
        private static bool initialized = false;

        // Giá vé cơ bản
        public static readonly Dictionary<BusClass, decimal> TicketPrices = new()
        {
            { BusClass.Standard, 300000m },    // Xe thường: 300k
            { BusClass.Premium, 400000m }     // Xe cao cấp: 400k
        };

        public MockTicketRepository()
        {
            // Initialize sample data on first instantiation
            if (!initialized)
            {
                InitializeSampleData();
                initialized = true;
            }
        }

        private static void InitializeSampleData()
        {
            var now = DateTime.Now;
            int ticketId = 1;

            // Create sample tickets for all popular routes
            var sampleTickets = new List<Ticket>();

            // Helper function to create multiple trips for a route
            void AddTripsForRoute(string diemDi, string diemDen, decimal giaBan, decimal giaCao, int numTrips = 15)
            {
                // Create trips for today and next 6 days
                for (int dayOffset = 0; dayOffset < 7; dayOffset++)
                {
                    var baseDate = now.Date.AddDays(dayOffset);
                    var startHour = 5;
                    var hoursPerTrip = 24 / numTrips;

                    for (int i = 0; i < numTrips; i++)
                    {
                        var hour = startHour + (i * hoursPerTrip);
                        if (hour >= 24) hour -= 24;

                        var isStandard = i % 3 != 2; // 2/3 standard, 1/3 premium
                        var gia = isStandard ? giaBan : giaCao;
                        var hangXe = isStandard ? BusClass.Standard : BusClass.Premium;

                        sampleTickets.Add(new Ticket
                        {
                            Id = ticketId++,
                            TenKhachHang = "",
                            SoDienThoai = "",
                            DiemDi = diemDi,
                            DiemDen = diemDen,
                            NgayDi = baseDate.AddHours(hour),
                            SoGhe = $"{(char)('A' + (i % 10))}{(i % 40) + 1}",
                            GiaVe = gia,
                            HangXe = hangXe
                        });
                    }
                }
            }

            // Add trips for each popular route (15 trips per day, 7 days = 105 trips per route)
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

            foreach (var ticket in sampleTickets)
            {
                tickets.Add(ticket);
            }
        }

        public List<Ticket> GetAll()
        {
            return new List<Ticket>(tickets);
        }

        public void Add(Ticket ticket)
        {
            ticket.Id = tickets.Count + 1;
            tickets.Add(ticket);
        }

        public void Update(Ticket ticket)
        {
            var existingTicket = tickets.FirstOrDefault(t => t.Id == ticket.Id);
            if (existingTicket != null)
            {
                existingTicket.TenKhachHang = ticket.TenKhachHang;
                existingTicket.SoDienThoai = ticket.SoDienThoai;
                existingTicket.DiemDi = ticket.DiemDi;
                existingTicket.DiemDen = ticket.DiemDen;
                existingTicket.NgayDi = ticket.NgayDi;
                existingTicket.SoGhe = ticket.SoGhe;
                existingTicket.GiaVe = ticket.GiaVe;
                existingTicket.HangXe = ticket.HangXe;
            }
        }

        public void Remove(Ticket ticket)
        {
            tickets.RemoveAll(t => t.Id == ticket.Id);
        }

        /// <summary>
        /// Lấy giá vé theo loại xe
        /// </summary>
        public static decimal GetTicketPrice(BusClass busClass)
        {
            return TicketPrices.TryGetValue(busClass, out var price) ? price : 300000m;
        }
    }
}