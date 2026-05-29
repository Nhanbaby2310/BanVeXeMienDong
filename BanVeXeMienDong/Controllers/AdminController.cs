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

        // 🗑️ Xóa user
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

        // 🗑️ Xóa vé
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _repository.GetAll().FirstOrDefault(t => t.Id == id);
            if (ticket == null)
                return NotFound();

            _repository.Remove(ticket);

            TempData["Success"] = "✅ Xóa vé thành công!";
            return RedirectToAction("Tickets");
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
