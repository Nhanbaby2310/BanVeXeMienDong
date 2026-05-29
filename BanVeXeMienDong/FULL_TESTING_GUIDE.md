# 🧪 HƯỚNG DẪN TEST TẤT CẢ CHỨC NĂNG

## 📋 MỤC LỤC
1. [Setup & Build](#setup--build)
2. [Test Trang Chủ](#test-trang-chủ)
3. [Test Mua Vé](#test-mua-vé)
4. [Test Giỏ Hàng](#test-giỏ-hàng)
5. [Test Thanh Toán](#test-thanh-toán)
6. [Test Tài Khoản](#test-tài-khoản)
7. [Test Admin](#test-admin)

---

## Setup & Build

### ✅ Bước 1: Build Project
```bash
dotnet build
```
✅ **Kỳ vọng**: Build successful, no errors

### ✅ Bước 2: Chạy Ứng Dụng
```bash
dotnet run
```
✅ **Kỳ vọng**: Application starts at https://localhost:7059 (hoặc port khác)

---

## Test Trang Chủ

### 🧪 Test 1: Hiển Thị Trang Chủ
**URL**: `https://localhost:7059/`

**Kiểm Tra**:
- [x] Hiển thị banner "Cam kết hoàn tiền..."
- [x] Hiển thị flash sale "Giảm đến 50%"
- [x] Form tìm kiếm có 4 trường: Điểm đi, Điểm đến, Ngày đi, Ngày về
- [x] Nút "Tìm kiếm" có thể click
- [x] 4 route cards hiển thị:
  - Sài Gòn - Cần Thơ (150.000đ)
  - Hà Nội - Hải Phòng (100.000đ)
  - Đà Nẵng - TP.HCM (450.000đ)
  - Hà Nội - Đà Nẵng (350.000đ)
- [x] 4 hình SVG chiếc xe khách hiển thị đúng màu

### 🧪 Test 2: Autocomplete Thành Phố
**Bước**:
1. Click vào field "Điểm xuất phát"
2. Gõ "Hà"

**Kiểm Tra**:
- [x] Dropdown hiển thị "Hà Nội", "Hải Phòng"
- [x] Có thể click chọn "Hà Nội"

### 🧪 Test 3: Swap Điểm Đi/Đến
**Bước**:
1. Điền "Hà Nội" → "HCM"
2. Click nút swap (mũi tên 2 chiều)

**Kiểm Tra**:
- [x] Fields hoán đổi: "HCM" → "Hà Nội"

### 🧪 Test 4: Submit Form Tìm Kiếm
**Bước**:
1. Điền: Hà Nội → HCM, chọn hôm nay
2. Click "Tìm kiếm"

**Kỳ Vọng**:
- [x] Navigate to `/Ticket/Index?diemDi=Hà Nội&diemDen=HCM&ngayDi=YYYY-MM-DD`
- [x] Hiển thị danh sách vé (nếu có)

---

## Test Mua Vé

### 🧪 Test 5: Flow Mua Vé (4 Bước)

#### Bước 1: Chọn Tuyến Đường
**URL**: `https://localhost:7059/Ticket/SelectRoute`

**Kiểm Tra**:
- [x] Hiển thị danh sách tuyến
- [x] Có nút/link chọn tuyến
- [x] Sau khi chọn → navigate to SelectDateTime

#### Bước 2: Chọn Ngày & Giờ
**URL**: `https://localhost:7059/Ticket/SelectDateTime`

**Kiểm Tra**:
- [x] Hiển thị danh sách ngày có sẵn
- [x] Có thể chọn ngày & giờ
- [x] Submit → SelectBusClass

#### Bước 3: Chọn Hạng Xe
**URL**: `https://localhost:7059/Ticket/SelectBusClass`

**Kiểm Tra**:
- [x] Hiển thị 2 loại: Standard & Premium
- [x] Có hình ảnh/icon xe
- [x] Có giá khác nhau
- [x] Click chọn → Create (chọn ghế)

#### Bước 4: Chọn Ghế
**URL**: `https://localhost:7059/Ticket/Create`

**Kiểm Tra**:
- [x] Bố cục ghế xe hiển thị (40 ghế)
- [x] Ghế đã đặt = màu xám (disabled)
- [x] Ghế trống = click được, highlight khi hover
- [x] Sau khi chọn → "Thêm vào giỏ"
- [x] Nút "Thêm vào giỏ" disabled nếu chưa chọn ghế

### 🧪 Test 6: API GetAvailableCities
**URL**: `https://localhost:7059/Ticket/GetAvailableCities`

**Kiểm Tra** (Open DevTools → Network tab):
- [x] Response = JSON array: ["Hà Nội", "HCM", "Đà Nẵng", ...]
- [x] Status 200

### 🧪 Test 7: API GetBookedSeats
**URL**: `https://localhost:7059/Ticket/GetBookedSeats?date=2025-01-20&busClass=0&diemDi=Hà Nội&diemDen=HCM`

**Kiểm Tra**:
- [x] Response = JSON array ghế đã đặt: ["A1", "B2", "C3"]
- [x] Status 200

---

## Test Giỏ Hàng

### 🧪 Test 8: Thêm Vé Vào Giỏ
**Bước**:
1. Mua vé (Test 5, Bước 4)
2. Click "Thêm vào giỏ"

**Kỳ Vọng**:
- [x] Toast/Alert: "✅ Thêm vào giỏ hàng thành công!"
- [x] Navigate to Cart

### 🧪 Test 9: Xem Giỏ Hàng
**URL**: `https://localhost:7059/Cart/Index`

**Kiểm Tra**:
- [x] Hiển thị list vé đã thêm
- [x] Mỗi vé show: Tuyến đường, ghế, giá, số lượng
- [x] Tính tổng tiền đúng
- [x] Có nút "Xóa", "Cập nhật số lượng"

### 🧪 Test 10: Xóa Vé Khỏi Giỏ
**Bước**:
1. Ở giỏ hàng, click nút "Xóa" trên một item

**Kỳ Vọng**:
- [x] Item biến mất
- [x] Toast: "✅ Xóa khỏi giỏ hàng thành công!"
- [x] Tổng tiền update

### 🧪 Test 11: Cập Nhật Số Lượng
**Bước**:
1. Ở giỏ hàng, thay đổi số lượng (e.g., từ 1 → 2)
2. Click "Cập nhật"

**Kỳ Vọng**:
- [x] Số lượng update
- [x] Tổng tiền tính lại

### 🧪 Test 12: Xóa Toàn Bộ Giỏ
**Bước**:
1. Click "Xóa tất cả" hoặc "Xóa giỏ"

**Kỳ Vọng**:
- [x] Giỏ rỗng
- [x] Message: "Giỏ hàng của bạn trống"

---

## Test Thanh Toán

### ✅ Điều Kiện Tiên Quyết
- [x] Phải đăng nhập (Test 15 trước)
- [x] Giỏ hàng có vé

### 🧪 Test 13: Trang Thanh Toán
**URL**: `https://localhost:7059/Checkout/Index`

**Kiểm Tra**:
- [x] Hiển thị summary giỏ hàng
- [x] Hiển thị tổng tiền
- [x] Form nhập:
  - Phương thức thanh toán (dropdown)
  - Số điện thoại
  - Email
  - Ghi chú (optional)
- [x] Nút "Thanh toán"

### 🧪 Test 14: Xử Lý Thanh Toán
**Bước**:
1. Chọn phương thức: "Thẻ Tín Dụng"
2. Nhập: 0901234567, abc@gmail.com
3. Click "Thanh toán"

**Kỳ Vọng**:
- [x] Tạo Order trong DB
- [x] Order có OrderCode: "ORDYYYYMMDDHHmmssUSERID"
- [x] ItemsJson = JSON của cart
- [x] Giỏ hàng được clear
- [x] Navigate to OrderConfirmation

### 🧪 Test 15: Xác Nhận Đơn Hàng
**URL**: `https://localhost:7059/Checkout/OrderConfirmation?orderId=1`

**Kiểm Tra**:
- [x] Hiển thị "Đơn hàng của bạn được xác nhận!"
- [x] Show OrderCode
- [x] Show Tổng tiền
- [x] Nút "Tiếp tục mua"

### 🧪 Test 16: Lịch Sử Đơn Hàng
**URL**: `https://localhost:7059/Checkout/MyOrders`

**Kiểm Tra**:
- [x] List tất cả order của user
- [x] Show: OrderCode, OrderDate, TotalAmount, Status
- [x] Click → OrderDetail
- [x] Chi tiết show các vé trong order

---

## Test Tài Khoản

### 🧪 Test 17: Đăng Ký
**URL**: `https://localhost:7059/Account/Register`

**Bước**:
1. Nhập: Username = "testuser", Password = "123456", Confirm = "123456"
2. Click "Đăng Ký"

**Kỳ Vọng**:
- [x] Validate password >= 6
- [x] Validate confirm password match
- [x] Check duplicate username
- [x] Hash password SHA256
- [x] Lưu vào DB
- [x] Redirect to Login với message "Đăng ký thành công!"

### 🧪 Test 18: Đăng Nhập
**URL**: `https://localhost:7059/Account/Login`

**Bước**:
1. Nhập: Username = "testuser", Password = "123456"
2. Click "Đăng Nhập"

**Kỳ Vọng**:
- [x] Kiểm tra username tồn tại
- [x] Kiểm tra password đúng (compare hash)
- [x] Set Session: "user", "userId", "userRole"
- [x] Redirect to Home
- [x] Navbar show "Đăng Xuất" + username

### 🧪 Test 19: Đăng Xuất
**Bước**:
1. Click "Đăng Xuất" ở navbar

**Kỳ Vọng**:
- [x] Session cleared
- [x] Navbar back to "Đăng Nhập"
- [x] Cart link ẩn

### 🧪 Test 20: Access Denied (Không Đăng Nhập)
**Bước**:
1. Không đăng nhập
2. Cố truy cập: `/Cart/Index`

**Kỳ Vọng**:
- [x] Redirect to `/Account/AccessDenied`
- [x] Message: "Bạn không có quyền truy cập!"

---

## Test Admin

### ✅ Điều Kiện Tiên Quyết
- [x] Phải là Admin (role = "Admin")
- [x] Default admin: username = "admin", password = "123456"

### 🧪 Test 21: Admin Dashboard
**URL**: `https://localhost:7059/Admin/Index`

**Kiểm Tra**:
- [x] Hiển thị thống kê:
  - Tổng Users
  - Admin count
  - User count
  - Tổng Tickets
  - Tổng Revenue
- [x] Nút/link quản lý: Users, Tickets, etc.

### 🧪 Test 22: Danh Sách Users
**URL**: `https://localhost:7059/Admin/Users`

**Kiểm Tra**:
- [x] List tất cả users
- [x] Show: ID, Username, Email, Role, Created Date
- [x] Nút "Chỉnh sửa" cho mỗi user
- [x] Nút "Xóa" (không xóa được ID=1 Admin)

### 🧪 Test 23: Chỉnh Sửa User
**URL**: `https://localhost:7059/Admin/EditUser/2`

**Bước**:
1. Thay đổi role từ "User" → "Admin"
2. Click "Lưu"

**Kỳ Vọng**:
- [x] Update role trong DB
- [x] Redirect to Users với message "Cập nhật thành công!"

### 🧪 Test 24: Danh Sách Vé
**URL**: `https://localhost:7059/Admin/Tickets`

**Kiểm Tra**:
- [x] List tất cả tickets
- [x] Show: ID, Tuyến đường, Ngày, Hạng, Ghế, Giá
- [x] Nút "Chỉnh giá"

### 🧪 Test 25: Chỉnh Sửa Giá Vé
**URL**: `https://localhost:7059/Admin/EditTicketPrice/1`

**Bước**:
1. Thay đổi giá từ 100000 → 120000
2. Click "Lưu"

**Kỳ Vọng**:
- [x] Update GiaVe trong DB
- [x] Redirect với message "Cập nhật giá thành công!"

---

## 🎯 CHECKLIST FINAL

- [ ] Build successful
- [ ] Trang chủ hiển thị đầy đủ
- [ ] Autocomplete thành phố hoạt động
- [ ] Flow mua vé 4 bước hoàn chỉnh
- [ ] Giỏ hàng add/remove/update đúng
- [ ] Thanh toán tạo Order
- [ ] Order confirmation hiển thị
- [ ] Lịch sử order hoạt động
- [ ] Đăng ký/Đăng nhập hoạt động
- [ ] Session persist đúng
- [ ] Admin dashboard hoạt động
- [ ] Admin manage users hoạt động
- [ ] Admin manage tickets hoạt động
- [ ] Authorization check hoạt động (Access Denied)
- [ ] Tất cả CSS/UI hiển thị đúng
- [ ] 4 route cards + SVG hiển thị đúng

---

## 🐛 Nếu Gặp Lỗi

### Lỗi: "Database connection failed"
```bash
# Check appsettings.json
# Verify SQL Server connection string
```

### Lỗi: "Session not working"
```csharp
// Check Program.cs
app.UseSession();  // PHẢI ở đây, trước MapControllerRoute
```

### Lỗi: "Authorization failed"
```csharp
// Check [Authorize("Admin")] attribute
// Check session key: "userRole"
```

### Lỗi: "Cart item không save"
```csharp
// Check JsonSerializerOptions
// Check Session key: "cart"
```

---

## 📞 Support

Nếu gặp vấn đề gì, check:
1. Build console output
2. Browser DevTools → Network tab
3. Browser Console → Errors
4. Database records (SQL Server Management Studio)
5. Session cookies (DevTools → Application → Cookies)

