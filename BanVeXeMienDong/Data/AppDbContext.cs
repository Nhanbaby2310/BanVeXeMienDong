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