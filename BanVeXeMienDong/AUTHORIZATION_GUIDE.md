# 🔐 Hệ Thống Phân Quyền & Quản Lý Người Dùng

## 📋 Tổng Quan

Hệ thống phân quyền (Authorization) cho phép kiểm soát ai có thể truy cập các tính năng nhất định trong ứng dụng dựa trên **Role** (Admin/User).

---

## 👥 Hai Loại Tài Khoản

### 1. **ADMIN** (Quản Trị Viên)
- ✅ Xem danh sách vé
- ✅ Tạo vé mới
- ✅ Truy cập **Admin Dashboard**
- ✅ Quản lý người dùng (chỉnh sửa, xóa)
- ✅ Xem thống kê

### 2. **USER** (Người Dùng Thường)
- ✅ Xem danh sách vé
- ✅ Tạo vé mới
- ❌ Không thể truy cập Admin Panel

---

## 🔑 Tài Khoản Test

| Username | Password | Role  |
|----------|----------|-------|
| admin    | 123456   | Admin |
| user     | 123456   | User  |

---

## 🎯 Các Tính Năng Phân Quyền

### ✅ Đã Cấu Hình

1. **Custom Authorize Attribute**
   - File: `Attributes/AuthorizeAttribute.cs`
   - Kiểm tra xem user đã đăng nhập hay chưa
   - Kiểm tra role của user
   - Chuyển hướng tới Login hoặc AccessDenied

2. **Ticket Controller** - Phân Quyền
   ```csharp
   [Authorize("Admin", "User")]  // Chỉ Admin & User
   public IActionResult SelectBusClass() { ... }
   
   [Authorize("Admin", "User")]  // Chỉ Admin & User
   public IActionResult Create() { ... }
   ```

3. **Admin Controller** - Chỉ Admin
   ```csharp
   [Authorize("Admin")]  // Chỉ Admin
   public class AdminController : Controller { ... }
   ```

4. **AccessDenied Page**
   - URL: `/Account/AccessDenied`
   - Hiển thị khi user không có quyền

---

## 🏢 Admin Panel

### 📊 Admin Dashboard
- **URL:** `/Admin/Index`
- **Chỉ cho Admin**
- Hiển thị:
  - 📈 Tổng số người dùng
  - 👮 Số lượng Admin
  - 👥 Số lượng User thường

### 👥 Quản Lý Người Dùng
- **URL:** `/Admin/Users`
- **Chỉ cho Admin**
- Xem danh sách tất cả người dùng
- ✏️ Chỉnh sửa user (đổi role)
- 🗑️ Xóa user (ngoại trừ admin #1)

### ✏️ Chỉnh Sửa Người Dùng
- **URL:** `/Admin/EditUser/{id}`
- **Chỉ cho Admin**
- Thay đổi username
- Thay đổi role (Admin/User)

---

## 🔄 Quy Trình Đăng Nhập & Phân Quyền

```
1. User truy cập /Account/Login
   ↓
2. Nhập username & password
   ↓
3. System hash password và so sánh
   ↓
4. Nếu đúng:
   - Lưu session: "user" = username
   - Lưu session: "role" = role (Admin/User)
   - Lưu session: "userId" = id
   - Chuyển hướng tới /Home/Index
   ↓
5. Nếu sai:
   - Hiển thị thông báo lỗi
   - Quay lại trang Login
```

---

## 🛡️ Cách Authorize Hoạt Động

### Custom Authorize Attribute

```csharp
[Authorize("Admin")]  // Chỉ Admin
public IActionResult AdminOnly() { ... }

[Authorize("Admin", "User")]  // Admin hoặc User
public IActionResult ForUsers() { ... }

[Authorize()]  // Chỉ cần đăng nhập
public IActionResult Protected() { ... }
```

### Quy Trình Kiểm Tra

```
1. Request đến action có [Authorize]
   ↓
2. Attribute kiểm tra session "user"
   ↓
3. Nếu không có session:
   - Chuyển hướng tới Login
   ↓
4. Nếu có session nhưng role không match:
   - Chuyển hướng tới AccessDenied
   ↓
5. Nếu role match:
   - Action được thực thi bình thường
```

---

## 📱 Navbar thể hiện thông tin

- Nếu **chưa đăng nhập**: Hiển thị "Đăng nhập" & "Đăng ký"
- Nếu **User đăng nhập**: Hiển thị tên user + role + "Đăng xuất"
- Nếu **Admin đăng nhập**: Hiển thị tên user + role + "Admin" link (màu đỏ) + "Đăng xuất"

---

## 🔐 Bảo Mật

✅ **Các biện pháp bảo mật đã áp dụng:**

1. **Session-based Authentication**
   - Session có timeout 30 phút
   - Cookie HttpOnly (không access từ JS)
   - Cookie IsEssential (bắt buộc)

2. **Password Hashing**
   - SHA256 hash
   - Không lưu plaintext password
   - Không thể recover mật khẩu gốc

3. **Authorization Checks**
   - Custom Attribute kiểm tra mỗi request
   - Chuyển hướng tới Login nếu cần
   - Hiển thị AccessDenied nếu không đủ quyền

4. **Data Protection**
   - Admin không thể xóa admin #1
   - Không thể thay đổi role của admin #1

---

## 🚀 Cách Thêm Phân Quyền cho Controller Khác

### Bước 1: Import Attribute
```csharp
using BanVeXeMienDong.Attributes;
```

### Bước 2: Thêm Decorator
```csharp
// Chỉ cho Admin
[Authorize("Admin")]
public class MyAdminController : Controller { ... }

// Chỉ cho Admin & User (ai đã đăng nhập)
[Authorize("Admin", "User")]
public IActionResult MyProtectedAction() { ... }

// Bất kỳ ai đã đăng nhập
[Authorize()]
public IActionResult ProtectedAction() { ... }
```

### Bước 3: Build & Test
```bash
dotnet build
dotnet run
```

---

## 📊 Database Schema

### Users Table
```
Id       | int (Primary Key)
Username | nvarchar
Password | nvarchar (SHA256 hash)
Role     | nvarchar (Admin/User)
```

---

## 🧪 Test Cases

### Test 1: Đăng Nhập Admin
1. Truy cập `/Account/Login`
2. Nhập: `admin` / `123456`
3. ✅ Chuyển hướng tới `/Home/Index`
4. ✅ Navbar hiển thị "Admin" link (màu đỏ)
5. ✅ Có thể truy cập `/Admin/Index`

### Test 2: Đăng Nhập User
1. Truy cập `/Account/Login`
2. Nhập: `user` / `123456`
3. ✅ Chuyển hướng tới `/Home/Index`
4. ✅ Navbar hiển thị user role là "User"
5. ❌ Không thể truy cập `/Admin/Index` (chuyển hướng AccessDenied)

### Test 3: Chưa Đăng Nhập
1. Truy cập `/Ticket/SelectBusClass` trực tiếp
2. ❌ Chuyển hướng tới `/Account/Login`

### Test 4: Quản Lý User (Admin)
1. Đăng nhập admin
2. Truy cập `/Admin/Users`
3. ✅ Hiển thị danh sách tất cả user
4. ✅ Có thể chỉnh sửa user
5. ✅ Có thể xóa user (ngoại trừ admin #1)

---

## ⚙️ Files Có Liên Quan

```
BanVeXeMienDong/
├── Attributes/
│   └── AuthorizeAttribute.cs          # Custom authorize logic
├── Controllers/
│   ├── AccountController.cs           # Login/Register/Logout
│   ├── AdminController.cs             # Admin functions
│   └── TicketController.cs            # (có [Authorize])
├── Views/
│   ├── Account/
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   └── AccessDenied.cshtml        # Trang không có quyền
│   ├── Admin/
│   │   ├── Index.cshtml               # Admin dashboard
│   │   ├── Users.cshtml               # Danh sách user
│   │   └── EditUser.cshtml            # Chỉnh sửa user
│   └── Shared/
│       └── _Layout.cshtml             # (có navbar logic)
└── Data/
    └── AppDbContext.cs                # Database context
```

---

## 🎓 Ví Dụ Sử Dụng

### Tạo trang chỉ cho Admin

```csharp
using BanVeXeMienDong.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BanVeXeMienDong.Controllers
{
    [Authorize("Admin")]  // 👈 Chỉ Admin
    public class ReportController : Controller
    {
        public IActionResult Dashboard()
        {
            // Chỉ admin mới có thể vào đây
            return View();
        }
    }
}
```

### Tạo trang cho Admin & User

```csharp
[Authorize("Admin", "User")]  // 👈 Admin & User
public IActionResult Booking()
{
    // Ai đã đăng nhập đều có thể vào
    return View();
}
```

### Tạo trang công khai

```csharp
// Không cần [Authorize]
public IActionResult Index()
{
    // Ai cũng có thể vào
    return View();
}
```

---

## 🐛 Xử Lý Lỗi

| Lỗi | Nguyên Nhân | Giải Pháp |
|-----|-----------|----------|
| "Trang không tìm thấy (404)" | Chưa đăng nhập | Truy cập `/Account/Login` |
| "Không có quyền truy cập" | Role không match | Đăng nhập với tài khoản có quyền |
| "Session hết hạn" | Quá 30 phút chưa hoạt động | Đăng nhập lại |

---

## 📚 Tài Liệu Liên Quan

- **Login Guide:** `LOGIN_GUIDE.md`
- **Authorization:** `Attributes/AuthorizeAttribute.cs`
- **Account Controller:** `Controllers/AccountController.cs`

---

**Hệ Thống Phân Quyền Đã Hoàn Thành! ✅**
