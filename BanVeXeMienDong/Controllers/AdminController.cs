using Microsoft.AspNetCore.Mvc;
using BanVeXeMienDong.Data;
using BanVeXeMienDong.Attributes;
using BanVeXeMienDong.Repositories;
using BanVeXeMienDong.Models;
using System.Text.Json;

namespace BanVeXeMienDong.Controllers
{
    [Authorize("Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITicketRepository _repository;

        public AdminController(AppDbContext context, ITicketRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // 📊 Dashboard admin
        public IActionResult Index()
        {
            var totalUsers = _context.Users.Count();
            var adminCount = _context.Users.Count(u => u.Role == "Admin");
            var userCount = _context.Users.Count(u => u.Role == "User");

            // Ticket statistics
            var tickets = _repository.GetAll();
            var totalTickets = tickets.Count;
            var totalRevenue = tickets.Sum(t => t.GiaVe);

            ViewBag.TotalUsers = totalUsers;
            ViewBag.AdminCount = adminCount;
            ViewBag.UserCount = userCount;
            ViewBag.TotalTickets = totalTickets;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }

        // 👥 Danh sách users
        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // ✏️ Chỉnh sửa user
        public IActionResult EditUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(int id, string username, string role)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            // Không cho phép thay đổi tài khoản admin đầu tiên
            if (user.Id == 1 && user.Role == "Admin")
            {
                if (role != "Admin")
                {
                    TempData["Error"] = "Không thể thay đổi role của Admin chính";
                    return View(user);
                }
            }

            user.Username = username;
            user.Role = role;
            _context.SaveChanges();

            TempData["Success"] = "Cập nhật người dùng thành công!";
            return RedirectToAction("Users");
        }

        // 🗑️ Xóa user (POST + AntiForgery - chống CSRF)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            // Không cho phép xóa admin đầu tiên
            if (user.Id == 1 && user.Role == "Admin")
            {
                TempData["Error"] = "Không thể xóa Admin chính";
                return RedirectToAction("Users");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["Success"] = "Xóa người dùng thành công!";
            return RedirectToAction("Users");
        }

        // 🎫 QUẢN LÝ VÉ - Xem danh sách vé đã đặt
        public IActionResult Tickets()
        {
            try
            {
                var tickets = _repository.GetAll().OrderByDescending(t => t.NgayDi).ToList();

                ViewBag.TotalTickets = tickets.Count;
                ViewBag.RevenueTotal = tickets.Sum(t => t.GiaVe);

                // Debug
                System.Diagnostics.Debug.WriteLine($"✅ Tickets count: {tickets.Count}");

                return View(tickets);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error in Tickets action: {ex.Message}");
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // 💰 QUẢN LÝ GIÁ VÉ - Sửa giá vé
        public IActionResult EditTicketPrice(int id)
        {
            var ticket = _repository.GetAll().FirstOrDefault(t => t.Id == id);
            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTicketPrice(int id, decimal giaVe)
        {
            var ticket = _repository.GetAll().FirstOrDefault(t => t.Id == id);
            if (ticket == null)
                return NotFound();

            // Debug: Check if giaVe is received correctly
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                TempData["Error"] = $"Lỗi xác thực: {errors}";
                return View(ticket);
            }

            if (giaVe <= 0)
            {
                TempData["Error"] = "Giá vé phải lớn hơn 0";
                return View(ticket);
            }

            ticket.GiaVe = giaVe;
            _repository.Update(ticket);

            TempData["Success"] = $"✅ Cập nhật giá vé thành công! Giá mới: {giaVe:N0} VND";
            return RedirectToAction("Tickets");
        }

        // 🗑️ Xóa vé (POST + AntiForgery - chống CSRF)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _repository.GetAll().FirstOrDefault(t => t.Id == id);
            if (ticket == null)
                return NotFound();

            _repository.Remove(ticket);

            TempData["Success"] = "✅ Xóa vé thành công!";
            return RedirectToAction("Tickets");
        }

        // 📊 THỐNG KÊ DOANH THU CHI TIẾT
        public IActionResult Revenue(string period = "month")
        {
            var allOrders = _context.Orders.Where(o => o.Status == "Confirmed").ToList();
            var now = DateTime.Now;

            // Tổng doanh thu
            ViewBag.TotalRevenue = allOrders.Sum(o => o.TotalAmount);
            ViewBag.TotalOrders = allOrders.Count;
            ViewBag.AvgOrderValue = allOrders.Count > 0 ? allOrders.Average(o => o.TotalAmount) : 0;

            // Doanh thu hôm nay
            var todayOrders = allOrders.Where(o => o.OrderDate.Date == now.Date).ToList();
            ViewBag.TodayRevenue = todayOrders.Sum(o => o.TotalAmount);
            ViewBag.TodayOrders = todayOrders.Count;

            // Doanh thu tuần này
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + 1);
            var weekOrders = allOrders.Where(o => o.OrderDate.Date >= startOfWeek).ToList();
            ViewBag.WeekRevenue = weekOrders.Sum(o => o.TotalAmount);
            ViewBag.WeekOrders = weekOrders.Count;

            // Doanh thu tháng này
            var monthOrders = allOrders.Where(o => o.OrderDate.Month == now.Month && o.OrderDate.Year == now.Year).ToList();
            ViewBag.MonthRevenue = monthOrders.Sum(o => o.TotalAmount);
            ViewBag.MonthOrders = monthOrders.Count;

            // Doanh thu theo ngày (7 ngày gần nhất) cho biểu đồ
            var dailyRevenue = new List<(string Date, decimal Amount, int Count)>();
            for (int i = 6; i >= 0; i--)
            {
                var date = now.Date.AddDays(-i);
                var dayOrders = allOrders.Where(o => o.OrderDate.Date == date).ToList();
                dailyRevenue.Add((date.ToString("dd/MM"), dayOrders.Sum(o => o.TotalAmount), dayOrders.Count));
            }
            ViewBag.DailyRevenue = dailyRevenue;

            // Doanh thu theo tháng (6 tháng gần nhất)
            var monthlyRevenue = new List<(string Month, decimal Amount, int Count)>();
            for (int i = 5; i >= 0; i--)
            {
                var month = now.AddMonths(-i);
                var mOrders = allOrders.Where(o => o.OrderDate.Month == month.Month && o.OrderDate.Year == month.Year).ToList();
                monthlyRevenue.Add(($"T{month.Month}/{month.Year % 100}", mOrders.Sum(o => o.TotalAmount), mOrders.Count));
            }
            ViewBag.MonthlyRevenue = monthlyRevenue;

            // Top tuyến đường doanh thu cao nhất
            var topRoutes = new List<(string Route, decimal Amount, int Count)>();
            foreach (var order in allOrders)
            {
                try
                {
                    var items = JsonSerializer.Deserialize<List<CartItem>>(order.ItemsJson);
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            topRoutes.Add((item.Route, item.Price, 1));
                        }
                    }
                }
                catch { }
            }
            ViewBag.TopRoutes = topRoutes
                .GroupBy(r => r.Route)
                .Select(g => new { Route = g.Key, Total = g.Sum(x => x.Amount), Count = g.Count() })
                .OrderByDescending(x => x.Total)
                .Take(10)
                .ToList();

            // Phương thức thanh toán phổ biến
            ViewBag.PaymentMethods = allOrders
                .GroupBy(o => o.PaymentMethod)
                .Select(g => new { Method = g.Key, Count = g.Count(), Total = g.Sum(o => o.TotalAmount) })
                .OrderByDescending(x => x.Count)
                .ToList();

            // Đơn hàng gần nhất
            ViewBag.RecentOrders = allOrders.OrderByDescending(o => o.OrderDate).Take(5).ToList();

            ViewBag.Period = period;
            return View();
        }

        // 📦 QUẢN LÝ ĐƠN HÀNG - Danh sách tất cả đơn hàng
        public IActionResult Orders(string status = "")
        {
            var orders = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                orders = orders.Where(o => o.Status == status);

            var orderList = orders.OrderByDescending(o => o.OrderDate).ToList();

            ViewBag.TotalOrders = orderList.Count;
            ViewBag.TotalRevenue = orderList.Where(o => o.Status == "Confirmed").Sum(o => o.TotalAmount);
            ViewBag.ConfirmedCount = orderList.Count(o => o.Status == "Confirmed");
            ViewBag.CancelledCount = orderList.Count(o => o.Status == "Cancelled");
            ViewBag.PendingCount = orderList.Count(o => o.Status == "Pending");
            ViewBag.CurrentFilter = status;

            return View(orderList);
        }

        // 📄 Chi tiết đơn hàng (Admin)
        public IActionResult OrderDetail(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();

            var user = _context.Users.FirstOrDefault(u => u.Id == order.UserId);
            ViewBag.Username = user?.Username ?? "Unknown";

            var items = JsonSerializer.Deserialize<List<CartItem>>(order.ItemsJson);
            ViewBag.Items = items;

            return View(order);
        }

        // ✅ Thay đổi trạng thái đơn hàng
        [HttpPost]
        public IActionResult UpdateOrderStatus(int id, string newStatus)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();

            var validStatuses = new[] { "Pending", "Confirmed", "Cancelled" };
            if (!validStatuses.Contains(newStatus))
            {
                TempData["Error"] = "Trạng thái không hợp lệ";
                return RedirectToAction("Orders");
            }

            order.Status = newStatus;
            _context.SaveChanges();

            TempData["Success"] = $"✅ Cập nhật trạng thái đơn #{order.OrderCode} → {newStatus}";
            return RedirectToAction("Orders");
        }
    }
}
