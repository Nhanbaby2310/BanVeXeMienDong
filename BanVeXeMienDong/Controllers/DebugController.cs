using Microsoft.AspNetCore.Mvc;
using BanVeXeMienDong.Repositories;

namespace BanVeXeMienDong.Controllers
{
    public class DebugController : Controller
    {
        private readonly ITicketRepository _ticketRepository;

        public DebugController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        // 🔍 DEBUG: Kiểm tra session
        public IActionResult CheckSession()
        {
            var user = HttpContext.Session.GetString("user");
            var role = HttpContext.Session.GetString("role");
            var userId = HttpContext.Session.GetInt32("userId");

            var debug = new
            {
                User = user ?? "NULL",
                Role = role ?? "NULL",
                UserId = userId ?? 0,
                IsAuthenticated = !string.IsNullOrEmpty(user)
            };

            return Json(debug);
        }

        // 🔍 DEBUG: Xóa session
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear();
            return Ok("Session cleared");
        }

        // 🔍 DEBUG: Xem tất cả tickets
        public IActionResult ViewAllTickets()
        {
            var tickets = _ticketRepository.GetAll();
            return Json(new 
            { 
                TotalTickets = tickets.Count,
                Tickets = tickets.Select(t => new {
                    Id = t.Id,
                    Customer = t.TenKhachHang,
                    Price = t.GiaVe,
                    Route = $"{t.DiemDi} -> {t.DiemDen}",
                    Date = t.NgayDi,
                    BusClass = t.HangXe.ToString()
                }).ToList()
            });
        }

        // 🔍 DEBUG: Test update ticket
        [HttpPost]
        public IActionResult TestUpdateTicket(int id, decimal newPrice)
        {
            var ticket = _ticketRepository.GetAll().FirstOrDefault(t => t.Id == id);
            if (ticket == null)
                return Json(new { Success = false, Message = "Ticket not found" });

            var oldPrice = ticket.GiaVe;
            ticket.GiaVe = newPrice;
            _ticketRepository.Update(ticket);

            // Verify update
            var updated = _ticketRepository.GetAll().FirstOrDefault(t => t.Id == id);

            return Json(new 
            { 
                Success = true,
                Message = $"Updated ticket {id}",
                OldPrice = oldPrice,
                NewPrice = newPrice,
                VerifiedPrice = updated?.GiaVe
            });
        }
    }
}
