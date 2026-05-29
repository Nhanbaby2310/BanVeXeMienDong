using BanVeXeMienDong.Repositories;
using BanVeXeMienDong.Data;
using BanVeXeMienDong.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 👉 THÊM DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 👉 THÊM SESSION
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian sống
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 👉 THÊM HTTP CONTEXT ACCESSOR & CART SERVICE
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICartService, CartService>();

// Repository
builder.Services.AddScoped<ITicketRepository, MockTicketRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 👉 UseSession PHẢI ở đây, trước MapControllerRoute
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();