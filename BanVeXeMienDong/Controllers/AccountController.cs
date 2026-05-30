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

        // 👤 TRANG HỒ SƠ CÁ NHÂN
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        // ✏️ CẬP NHẬT HỒ SƠ
        [HttpPost]
        public IActionResult Profile(string fullName, string email, string phone)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null)
                return RedirectToAction("Login");

            user.FullName = fullName;
            user.Email = email;
            user.Phone = phone;
            _context.SaveChanges();

            TempData["Success"] = "✅ Cập nhật hồ sơ thành công!";
            return View(user);
        }

        // 🔑 ĐỔI MẬT KHẨU
        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (!userId.HasValue)
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);
            if (user == null)
                return RedirectToAction("Login");

            // Verify current password
            if (Hash(currentPassword) != user.Password)
            {
                TempData["Error"] = "Mật khẩu hiện tại không đúng";
                return RedirectToAction("Profile");
            }

            if (newPassword != confirmNewPassword)
            {
                TempData["Error"] = "Mật khẩu mới không khớp";
                return RedirectToAction("Profile");
            }

            if (newPassword.Length < 6)
            {
                TempData["Error"] = "Mật khẩu mới phải có ít nhất 6 ký tự";
                return RedirectToAction("Profile");
            }

            user.Password = Hash(newPassword);
            _context.SaveChanges();

            TempData["Success"] = "✅ Đổi mật khẩu thành công!";
            return RedirectToAction("Profile");
        }

        // 🔓 QUÊN MẬT KHẨU
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string username, string email)
        {
            // Tìm user bằng username (email là tùy chọn - nếu có thì kiểm tra thêm)
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // Nếu có nhập email và user có email → kiểm tra khớp
            if (user != null && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(user.Email))
            {
                if (user.Email.ToLower() != email.ToLower())
                {
                    user = null; // Email không khớp
                }
            }

            if (user == null)
            {
                ViewBag.Error = "Không tìm thấy tài khoản với thông tin này. Vui lòng kiểm tra lại tên đăng nhập.";
                return View();
            }

            // Generate reset token
            var token = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddHours(1);
            _context.SaveChanges();

            ViewBag.Success = $"Mã đặt lại mật khẩu của bạn là: {token} (có hiệu lực 1 giờ). Hãy nhập mã này bên dưới.";
            ViewBag.ShowResetForm = true;
            ViewBag.Username = username;
            ViewBag.Token = token; // Hiển thị token trực tiếp (vì đây là demo, không gửi email thật)
            return View();
        }

        // 🔄 RESET MẬT KHẨU
        [HttpPost]
        public IActionResult ResetPassword(string username, string token, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp";
                ViewBag.ShowResetForm = true;
                ViewBag.Username = username;
                return View("ForgotPassword");
            }

            if (newPassword.Length < 6)
            {
                ViewBag.Error = "Mật khẩu mới phải có ít nhất 6 ký tự";
                ViewBag.ShowResetForm = true;
                ViewBag.Username = username;
                return View("ForgotPassword");
            }

            var user = _context.Users.FirstOrDefault(u =>
                u.Username == username &&
                u.ResetToken == token &&
                u.ResetTokenExpiry > DateTime.Now);

            if (user == null)
            {
                ViewBag.Error = "Mã đặt lại không hợp lệ hoặc đã hết hạn";
                ViewBag.ShowResetForm = true;
                ViewBag.Username = username;
                return View("ForgotPassword");
            }

            user.Password = Hash(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            _context.SaveChanges();

            TempData["Success"] = "✅ Đặt lại mật khẩu thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }
    }
}