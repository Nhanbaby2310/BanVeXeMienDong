# 📖 Hướng Dẫn Sử Dụng Tính Năng Đăng Nhập & Đăng Ký

## ✅ Tính Năng Đã Cấu Hình

### 1. **Đăng Ký Tài Khoản** (`/Account/Register`)
- ✅ Kiểm tra tài khoản không trống
- ✅ Kiểm tra mật khẩu không trống
- ✅ Xác nhận mật khẩu (phải trùng khớp)
- ✅ Mật khẩu tối thiểu 6 ký tự
- ✅ Kiểm tra tài khoản không trùng lặp
- ✅ Mật khẩu được hash SHA256 trước khi lưu

### 2. **Đăng Nhập** (`/Account/Login`)
- ✅ Kiểm tra tài khoản/mật khẩu không trống
- ✅ So sánh mật khẩu hash
- ✅ Lưu thông tin session (username, role)
- ✅ Chuyển hướng tới Home sau khi đăng nhập

### 3. **Đăng Xuất** (`/Account/Logout`)
- ✅ Xóa session
- ✅ Chuyển hướng tới Login

### 4. **Giao Diện**
- ✅ Hiện đại với gradient màu
- ✅ Responsive design (mobile-friendly)
- ✅ Thông báo lỗi rõ ràng
- ✅ Navigation bar hiển thị trạng thái đăng nhập

---

## 🔑 Tài Khoản Test

Hai tài khoản đã được tạo sẵn (mật khẩu: `123456`):

| Username | Password | Role  |
|----------|----------|-------|
| admin    | 123456   | Admin |
| user     | 123456   | User  |

---

## 🏗️ Cấu Trúc Dự Án

```
BanVeXeMienDong/
├── Controllers/
│   └── AccountController.cs          # Logic đăng nhập/đăng ký
├── Models/
│   └── User.cs                       # Model User
├── Data/
│   └── AppDbContext.cs               # Entity Framework DbContext
├── Views/
│   └── Account/
│       ├── Login.cshtml              # View đăng nhập
│       └── Register.cshtml           # View đăng ký
├── Migrations/                       # Entity Framework migrations
└── Program.cs                        # Cấu hình ứng dụng
```

---

## 🔧 Các Tệp Đã Cập Nhật

### 1. **Program.cs**
- ✅ Thêm `AddDbContext` cho SQL Server
- ✅ Thêm `AddSession` cho session management
- ✅ Gọi `UseSession()` trước routing

### 2. **AccountController.cs**
- ✅ Hash password với SHA256
- ✅ Validate input trước khi xử lý
- ✅ Kiểm tra user tồn tại
- ✅ Session management

### 3. **Login.cshtml & Register.cshtml**
- ✅ Giao diện hiện đại
- ✅ Các trường form đầy đủ
- ✅ Thông báo lỗi/thành công

### 4. **appsettings.json**
- ✅ Connection string đến LocalDB

### 5. **AppDbContext.cs**
- ✅ Seed data test (admin + user)

---

## 📋 Hướng Dẫn Sử Dụng

### **Chạy Ứng Dụng:**
```bash
cd BanVeXeMienDong/BanVeXeMienDong
dotnet run
```
Ứng dụng sẽ chạy tại `https://localhost:5001`

### **Đăng Ký Tài Khoản Mới:**
1. Truy cập `/Account/Register`
2. Nhập tài khoản, mật khẩu (tối thiểu 6 ký tự)
3. Xác nhận mật khẩu
4. Nhấn "Đăng ký"

### **Đăng Nhập:**
1. Truy cập `/Account/Login`
2. Nhập tài khoản và mật khẩu
3. Nhấn "Đăng nhập"
4. Nếu thành công sẽ chuyển hướng tới Home

### **Kiểm Tra Đăng Nhập:**
- Nhìn vào navbar, nếu thấy tên user và "Đăng xuất" = đã đăng nhập ✅
- Nếu thấy "Đăng nhập" và "Đăng ký" = chưa đăng nhập

---

## 🔒 Bảo Mật

- ✅ Mật khẩu được hash SHA256 (không lưu plaintext)
- ✅ Session cookie có `HttpOnly = true`
- ✅ Validate dữ liệu đầu vào
- ✅ Kiểm tra tài khoản tồn tại

---

## 📊 Database

**Connection String:** `(localdb)\mssqllocaldb`
**Database Name:** `BanVeXeDB`
**Table:** Users (Id, Username, Password, Role)

Để xem database:
- Visual Studio → SQL Server Object Explorer → BanVeXeDB → Tables → dbo.Users

---

## ⚠️ Lỗi Thường Gặp

| Lỗi | Nguyên Nhân | Giải Pháp |
|-----|-----------|----------|
| "Cannot connect to server" | LocalDB chưa chạy | Mở SQL Server Management Studio |
| "Session lost" | Cookie bị xóa | Đăng nhập lại |
| "Account not found" | Nhập sai tài khoản | Kiểm tra lại tài khoản |

---

## 🚀 Mở Rộng

Bạn có thể thêm:
- ✨ Email confirmation
- ✨ Password recovery
- ✨ Two-factor authentication
- ✨ Role-based authorization
- ✨ OAuth (Google, Facebook login)

---

**Hệ Thống đăng nhập & đăng ký đã hoàn toàn sẵn sàng để sử dụng! 🎉**
