using Microsoft.AspNetCore.Mvc;
using BanVeXeMienDong.Models;
using BanVeXeMienDong.Data;
using System.Security.Cryptography;
using System.Text;

namespace BanVeXeMienDong.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // HASH PASSWORD
        private string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user, string confirmPassword)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                ViewBag.Error = "Tài khoản và mật khẩu không được để trống";
                return View();
            }

            // Kiểm tra mật khẩu xác nhận
            if (user.Password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp";
                return View();
            }

            // Kiểm tra độ dài mật khẩu
            if (user.Password.Length < 6)
            {
                ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự";
                return View();
            }

            // Kiểm tra user đã tồn tại
            var existingUser = _context.Users.FirstOrDefault(x => x.Username == user.Username);
            if (existingUser != null)
            {
                ViewBag.Error = "Tài khoản này đã tồn tại";
                return View();
            }

            user.Password = Hash(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();

            ViewBag.Success = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                ViewBag.Error = "Vui lòng nhập tài khoản và mật khẩu";
                return View();
            }

            var pass = Hash(user.Password);

            var u = _context.Users
                .FirstOrDefault(x => x.Username == user.Username && x.Password == pass);

            if (u != null)
            {
                HttpContext.Session.SetString("user", u.Username);
                HttpContext.Session.SetString("role", u.Role);
                HttpContext.Session.SetInt32("userId", u.Id);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // 👉 PAGE KHÔNG CÓ QUYỀN TRUY CẬP
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}