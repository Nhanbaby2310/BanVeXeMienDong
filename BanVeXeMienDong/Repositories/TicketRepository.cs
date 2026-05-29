using BanVeXeMienDong.Data;
using BanVeXeMienDong.Models;
using Microsoft.EntityFrameworkCore;

namespace BanVeXeMienDong.Repositories
{
    /// <summary>
    /// Repository thực sử dụng EF Core - dữ liệu được persist vào SQL Server
    /// </summary>
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        // Giá vé cơ bản (giống MockTicketRepository để tương thích ngược)
        public static readonly Dictionary<BusClass, decimal> TicketPrices = new()
        {
            { BusClass.Standard, 300000m },    // Xe thường: 300k
            { BusClass.Premium, 400000m }      // Xe cao cấp: 400k
        };

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Ticket> GetAll()
        {
            return _context.Tickets.AsNoTracking().ToList();
        }

        public void Add(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public void Update(Ticket ticket)
        {
            var existingTicket = _context.Tickets.FirstOrDefault(t => t.Id == ticket.Id);
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
                _context.SaveChanges();
            }
        }

        public void Remove(Ticket ticket)
        {
            var existingTicket = _context.Tickets.FirstOrDefault(t => t.Id == ticket.Id);
            if (existingTicket != null)
            {
                _context.Tickets.Remove(existingTicket);
                _context.SaveChanges();
            }
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
