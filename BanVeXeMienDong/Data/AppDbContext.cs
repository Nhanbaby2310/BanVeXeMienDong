using Microsoft.EntityFrameworkCore;
using BanVeXeMienDong.Models;

namespace BanVeXeMienDong.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Thêm dữ liệu mặc định (test data)
            // Mật khẩu được hash bằng BCrypt (có salt tự động, an toàn hơn SHA256)
            // Mật khẩu: "quanly"
            // Mật khẩu: "123456"
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Username = "quanly", 
                    Password = "$2b$12$snU6xHH.nozC1IgEE6cn6eAVOsFOzur1osYECANDaWkb3WeUDCGEu",
                    Role = "Admin"
                },
                new User 
                { 
                    Id = 2, 
                    Username = "user", 
                    Password = "$2b$12$LN/7WwlEt7bdf7DgcEXpsOPxpiQSj1ImEiU7sYI2slFep84O7wAN.",
                    Role = "User"
                }
            );
        }
    }
}