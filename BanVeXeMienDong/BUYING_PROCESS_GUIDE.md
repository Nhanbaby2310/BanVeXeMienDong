# 🛒 Quy Trình Mua Vé - Giỏ Hàng & Thanh Toán

## 📋 Tóm Tắt Quy Trình

```
1. Đăng Nhập
   ↓
2. Chọn Loại Xe (Standard/Premium)
   ↓
3. Chọn Ghế & Nhập Thông Tin
   ↓
4. ✅ THÊM VÀO GIỎ HÀNG (không lưu database)
   ↓
5. Xem Giỏ Hàng
   ↓
6. Thanh Toán
   ↓
7. Xác Nhận & Lưu Database
   ↓
8. Xem Lịch Sử Đơn Hàng
```

---

## 🚀 Hướng Dẫn Chi Tiết

### **Bước 1: Đăng Nhập**
1. Truy cập `/Account/Login`
2. Nhập tài khoản: `quanly` / `quanly` (hoặc `user` / `123456`)
3. Nhấn "Đăng nhập"

### **Bước 2: Chọn Loại Xe**
1. Navbar → "Bán vé mới"
2. Hoặc URL: `/Ticket/SelectBusClass`
3. Chọn **Standard** hoặc **Premium**

### **Bước 3: Chọn Ghế & Nhập Thông Tin**
1. Chọn **ghế ngồi** trên sơ đồ (hiển thị xanh = trống)
2. Điều chỉnh số lượng vé nếu cần
3. Nhập thông tin:
   - 🚩 **Điểm Đi** (TP.HCM, Hà Nội, v.v.)
   - 📍 **Điểm Đến**
   - 👤 **Tên Khách Hàng**
   - 📱 **Số Điện Thoại**
   - 💰 **Giá Vé** (VND)
4. Nhấn **"Thêm vào Giỏ Hàng"** ← ĐIỂM KHÁC

### **Bước 4: Xem Giỏ Hàng**
1. **Tự động** chuyển hướng tới `/Cart/Index`
2. Hoặc Navbar → **"Giỏ Hàng"**
3. Xem danh sách vé đã thêm:
   - 🛣️ Tuyến đường
   - 💺 Ghế
   - 🚌 Loại xe
   - ⏰ Giờ khởi hành
   - 💵 Giá & Tổng tiền

### **Bước 5: Cập Nhật Giỏ Hàng (Tùy Chọn)**
- ✏️ **Cập nhật số lượng** → Nhập số mới → ✓ (hoặc không, mặc định là 1)
- 🗑️ **Xóa item** → Click "Xóa"
- 🗑️ **Xóa tất cả** → Click "Xóa Tất Cả"

### **Bước 6: Thanh Toán**
1. Từ giỏ hàng → Nhấn **"Thanh Toán"**
2. Hoặc URL: `/Checkout/Index`
3. Giao diện thanh toán hiển thị:
   - 📋 **Tóm Tắt Đơn Hàng** (bên trái)
   - 💳 **Form Thanh Toán** (bên phải)

### **Bước 7: Nhập Thông Tin Thanh Toán**
1. **Chọn Phương Thức Thanh Toán:**
   - 💵 Thanh Toán Tiền Mặt
   - 💳 Thẻ Tín Dụng
   - 🏦 Chuyển Khoản Ngân Hàng
   - 📱 Ví Điện Tử

2. **Nhập Thông Tin Liên Lạc:**
   - 📱 **Số Điện Thoại** (bắt buộc)
   - 📧 **Email** (bắt buộc)
   - 📝 **Ghi Chú** (tùy chọn)

3. **Đồng Ý Điều Khoản:**
   - ☑️ "Tôi đồng ý với điều khoản và điều kiện"

4. **Nhấn "Xác Nhận Thanh Toán"**

### **Bước 8: Xác Nhận Đơn Hàng**
1. Hiển thị trang **Xác Nhận Thành Công** ✅
2. Xem thông tin:
   - ✅ **Mã Đơn Hàng** (ORD20260328...)
   - 📅 **Ngày Đặt**
   - 💳 **Phương Thức Thanh Toán**
   - 📱 **Số Điện Thoại**
   - 📧 **Email**
   - 💰 **Tổng Cộng**
   - 🎫 **Thông Tin Vé Đã Mua**

3. **Bước Tiếp Theo:**
   - Vé sẽ được gửi qua email trong 5 phút
   - Kiểm tra email để nhận vé
   - Lưu mã đơn hàng

### **Bước 9: Xem Lịch Sử Đơn Hàng**
1. Navbar → **"Đơn Hàng"**
2. Hoặc URL: `/Checkout/MyOrders`
3. Xem tất cả đơn hàng:
   - 📦 Mã Đơn Hàng
   - 📅 Ngày Đặt
   - 💳 Phương Thức
   - 📱 SĐT
   - 💰 Tổng Tiền
   - 📊 Trạng Thái (Confirmed/Cancelled)

4. **Tùy Chọn:**
   - 👁️ **Xem Chi Tiết** → Xem toàn bộ thông tin đơn
   - ❌ **Hủy Đơn** → Hủy (chỉ khi trạng thái Confirmed)

---

## 🔄 So Sánh Cũ vs Mới

### **Cách Cũ (Trước):**
```
Chọn Vé → Lưu Trực Tiếp vào Database
```
❌ Không có giỏ hàng
❌ Không thể xem trước
❌ Không thể thay đổi

### **Cách Mới (Bây Giờ):**
```
Chọn Vé → Thêm vào Giỏ Hàng (Session)
      ↓
  Xem Giỏ Hàng
      ↓
  Cập Nhật/Xóa
      ↓
  Thanh Toán
      ↓
  Lưu vào Database + Email
```
✅ Có giỏ hàng
✅ Xem trước được
✅ Thay đổi được
✅ Lưu trữ đơn hàng

---

## 💾 Dữ Liệu Lưu Trữ

### **Trong Quá Trình Chọn & Giỏ:**
- **Session** (RAM)
- Tạm thời lưu giỏ hàng
- Sẽ mất nếu đóng trình duyệt

### **Sau Thanh Toán:**
- **Database** (SQL Server)
- Lưu vĩnh viễn
- Có thể xem lại sau

---

## ✅ Những Thay Đổi Trong Code

### **TicketController.cs**
```csharp
// Trước: _repository.Add(ticket);
// Sau:
var cartItem = new CartItem
{
    TicketCode = "TK" + DateTime.Now.Ticks,
    Route = $"{ticket.DiemDi} → {ticket.DiemDen}",
    Seats = ticket.SoGhe,
    Price = ticket.GiaVe,
    Quantity = 1,
    BusClass = ticket.HangXe.ToString(),
    DepartureTime = ticket.NgayDi.ToString("dd/MM/yyyy HH:mm")
};

_cartService.AddToCart(cartItem);
return RedirectToAction("Index", "Cart");
```

---

## 📊 Trạng Thái Đơn Hàng

| Trạng Thái | Ý Nghĩa | Có Thể Hủy |
|-----------|---------|----------|
| Confirmed | Đã xác nhận thanh toán | ✅ Có |
| Cancelled | Đã bị hủy | ❌ Không |
| Pending   | Chờ xác nhận (chưa dùng) | ✅ Có |

---

## 🔐 Bảo Mật & Lưu Ý

✅ **Vé trong giỏ:**
- Chỉ lưu trong Session (tạm thời)
- Mất nếu logout hoặc đóng browser
- Mã vé tạo tự động (không trùng)

✅ **Sau Thanh Toán:**
- Lưu vĩnh viễn vào database
- Gửi email xác nhận
- Mã đơn hàng duy nhất (ORD...)

⚠️ **Lưu Ý:**
- Ghế đã chọn trong giỏ **chưa được khóa**
- Thanh toán xong mới khóa vĩnh viễn
- Nếu 2 người chọn ghế giống nhau, người nào thanh toán trước được cái đó

---

## 🧪 Kịch Bản Test

### **Test 1: Thêm Vé Cơ Bản**
1. ✅ Đăng nhập → Chọn vé → Thêm giỏ
2. ✅ Kiểm tra giỏ hàng hiển thị vé
3. ✅ Kiểm tra tổng tiền đúng

### **Test 2: Thêm Nhiều Vé**
1. ✅ Thêm vé 1
2. ✅ Quay lại thêm vé 2
3. ✅ Giỏ hiển thị 2 vé
4. ✅ Tổng tiền = vé1 + vé2

### **Test 3: Cập Nhật & Xóa**
1. ✅ Thêm vé → Cập nhật qty → Kiểm tra tổng
2. ✅ Xóa 1 item → Giỏ cập nhật
3. ✅ Xóa tất cả → Giỏ trống

### **Test 4: Thanh Toán**
1. ✅ Chọn phương thức
2. ✅ Nhập SĐT & Email
3. ✅ Thanh toán → Order lưu vào DB
4. ✅ Giỏ xóa tự động

### **Test 5: Xem Lịch Sử**
1. ✅ Vào My Orders
2. ✅ Thấy đơn vừa tạo
3. ✅ Xem chi tiết
4. ✅ Hủy đơn (nếu muốn)

---

## 🎯 Tính Năng Sắp Tới

- 📧 Tích hợp email xác nhận
- 📄 Tạo PDF Invoice
- 💳 Gateway thanh toán thực (Stripe, Paypal)
- 🎁 Mã voucher/discount
- ⭐ Review vé
- 📊 Dashboard quản lý vé

---

**Quy Trình Mua Vé Hoàn Toàn Mới! 🎉**
