using Microsoft.EntityFrameworkCore;
using BanVeXeMienDong.Models;

namespace BanVeXeMienDong.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình bảng Ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.TenKhachHang).HasMaxLength(200);
                entity.Property(t => t.SoDienThoai).HasMaxLength(20);
                entity.Property(t => t.DiemDi).IsRequired().HasMaxLength(100);
                entity.Property(t => t.DiemDen).IsRequired().HasMaxLength(100);
                entity.Property(t => t.SoGhe).IsRequired().HasMaxLength(500);
                entity.Property(t => t.GiaVe).HasColumnType("decimal(18,2)");
                entity.Property(t => t.HangXe).HasConversion<int>();

                // Index để tìm kiếm nhanh theo tuyến đường + ngày
                entity.HasIndex(t => new { t.DiemDi, t.DiemDen, t.NgayDi });
            });

            // Cấu hình bảng Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            });

            // Thêm dữ liệu mặc định (test data)
            // Mật khẩu: "quanly" được hash thành: NKN46HZxLgle0QWaTuROoqx8k4Md1PWOXpgIU6a3mZ4=
            // Mật khẩu: "123456" được hash thành: N9XB5N9pqH5Vn/F0LrfWvJwFWYW+Xc2yXgNM7f2RJIc=
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Username = "quanly", 
                    Password = "NKN46HZxLgle0QWaTuROoqx8k4Md1PWOXpgIU6a3mZ4=",
                    Role = "Admin"
                },
                new User 
                { 
                    Id = 2, 
                    Username = "user", 
                    Password = "N9XB5N9pqH5Vn/F0LrfWvJwFWYW+Xc2yXgNM7f2RJIc=",
                    Role = "User"
                }
            );
        }
    }
}