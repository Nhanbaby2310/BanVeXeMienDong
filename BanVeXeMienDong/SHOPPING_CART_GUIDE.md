# 🛒 Hệ Thống Giỏ Hàng & Thanh Toán

## 📋 Tổng Quan

Hệ thống giỏ hàng và thanh toán cho phép người dùng:
- ✅ Thêm vé vào giỏ hàng
- ✅ Xem và quản lý giỏ hàng
- ✅ Thanh toán đơn hàng
- ✅ Xem lịch sử đơn hàng

---

## 🎯 Tính Năng Chính

### 1. **Giỏ Hàng (Cart)**
- 📍 URL: `/Cart/Index`
- ✅ Xem tất cả vé đã thêm
- ✅ Cập nhật số lượng
- ✅ Xóa item khỏi giỏ
- ✅ Xem tổng tiền
- ✅ Lưu trong Session (không cần database)

### 2. **Thanh Toán (Checkout)**
- 📍 URL: `/Checkout/Index`
- ✅ Chọn phương thức thanh toán (Cash, Card, Bank, Wallet)
- ✅ Nhập thông tin liên lạc (SĐT, Email)
- ✅ Thêm ghi chú
- ✅ Xác nhận thanh toán
- ✅ Lưu vào database

### 3. **Xác Nhận Đơn Hàng**
- 📍 URL: `/Checkout/OrderConfirmation/{id}`
- ✅ Hiển thị mã đơn hàng
- ✅ Chi tiết vé đã mua
- ✅ Thông tin thanh toán

### 4. **Lịch Sử Đơn Hàng**
- 📍 URL: `/Checkout/MyOrders`
- ✅ Xem tất cả đơn hàng của user
- ✅ Trạng thái đơn (Confirmed, Cancelled)
- ✅ Xem chi tiết từng đơn
- ✅ Hủy đơn (nếu còn Confirmed)

---

## 🏗️ Cấu Trúc Code

### **Models**
```
CartItem.cs       - Item trong giỏ hàng
Order.cs          - Đơn hàng trong database
```

### **Services**
```
CartService.cs    - Quản lý giỏ hàng (Session-based)
  - AddToCart()
  - RemoveFromCart()
  - UpdateCart()
  - GetCart()
  - ClearCart()
  - GetTotal()
  - GetCartCount()
```

### **Controllers**
```
CartController.cs
  - Index()           - Xem giỏ hàng
  - AddToCart()       - Thêm item
  - RemoveFromCart()  - Xóa item
  - UpdateCart()      - Cập nhật số lượng
  - ClearCart()       - Xóa toàn bộ

CheckoutController.cs
  - Index()           - Trang thanh toán
  - ProcessPayment()  - Xử lý thanh toán
  - OrderConfirmation() - Xác nhận
  - MyOrders()        - Danh sách đơn
  - OrderDetail()     - Chi tiết đơn
  - CancelOrder()     - Hủy đơn
```

### **Views**
```
Cart/Index.cshtml                - Giỏ hàng
Checkout/Index.cshtml           - Thanh toán
Checkout/OrderConfirmation.cshtml - Xác nhận
Checkout/MyOrders.cshtml        - Lịch sử đơn
```

---

## 🚀 Hướng Dẫn Sử Dụng

### **Bước 1: Đăng Nhập**
```
1. Truy cập /Account/Login
2. Đăng nhập bằng tài khoản admin/user
```

### **Bước 2: Chọn Vé**
```
1. Truy cập /Ticket/Index
2. Chọn loại xe (Standard/Premium)
3. Chọn ghế ngồi
4. Nhấn "Thêm vào Giỏ Hàng"
```

### **Bước 3: Xem Giỏ Hàng**
```
1. Navbar → "Giỏ Hàng" hoặc /Cart/Index
2. Xem danh sách vé
3. Cập nhật số lượng nếu cần
4. Xóa item nếu cần
```

### **Bước 4: Thanh Toán**
```
1. Từ giỏ hàng → Click "Thanh Toán"
2. Chọn phương thức thanh toán
3. Nhập SĐT và Email
4. Nhấn "Xác Nhận Thanh Toán"
```

### **Bước 5: Xem Xác Nhận**
```
1. Hiển thị mã đơn hàng
2. Xem chi tiết vé đã mua
3. Link đến "Đơn Hàng của Tôi"
```

### **Bước 6: Quản Lý Đơn Hàng**
```
1. Navbar → "Đơn Hàng" hoặc /Checkout/MyOrders
2. Xem tất cả đơn hàng
3. Click "Xem Chi Tiết" để xem đơn
4. Hủy đơn nếu cần
```

---

## 📊 Quy Trình Mua Hàng

```
┌─────────────┐
│   Đăng Nhập │
└──────┬──────┘
       │
       ▼
┌──────────────────┐
│  Xem Danh Sách   │
│      Vé Xe       │
└──────┬───────────┘
       │
       ▼
┌──────────────────┐      ┌────────────────┐
│  Chọn Loại Xe &  │─────>│   Cập Nhật &   │
│   Ghế Ngồi       │      │  Xóa Item      │
└──────┬───────────┘      └────────────────┘
       │
       ▼
┌──────────────────┐
│  Thêm vào Giỏ    │
│      Hàng        │
└──────┬───────────┘
       │
       ▼
┌──────────────────┐
│  Xem Giỏ Hàng    │
│   & Tính Tiền    │
└──────┬───────────┘
       │
       ▼
┌──────────────────┐
│     Thanh Toán   │
│  (Chọn Phương    │
│   Thức & Info)   │
└──────┬───────────┘
       │
       ▼
┌──────────────────┐
│  Xác Nhận Đơn    │
│   & Lưu Database │
└──────┬───────────┘
       │
       ▼
┌──────────────────┐
│ Xem Lịch Sử      │
│ & Chi Tiết Đơn   │
└──────────────────┘
```

---

## 💾 Dữ Liệu Lưu Trữ

### **Session (Tạm Thời)**
```
{
  "cart": [
    {
      "id": 1,
      "ticketCode": "TK001",
      "route": "TP.HCM - Hà Nội",
      "seats": "A1, A2",
      "price": 500000,
      "quantity": 2,
      "busClass": "Standard",
      "departureTime": "14:30"
    }
  ]
}
```

### **Database (Orders Table)**
```
Id            | int (Primary Key)
UserId        | int (FK)
OrderCode     | nvarchar (ORD20260328...)
OrderDate     | datetime2
TotalAmount   | decimal
Status        | nvarchar (Confirmed/Cancelled)
ItemsJson     | nvarchar (JSON array)
PaymentMethod | nvarchar (Cash/Card/Bank/Wallet)
PhoneNumber   | nvarchar
Email         | nvarchar
```

---

## 🔐 Bảo Mật

✅ **Các biện pháp:**
- ✅ Chỉ user đã đăng nhập mới có thể mua hàng (`[Authorize]`)
- ✅ User chỉ xem được đơn của chính mình
- ✅ Session-based cart (không lưu trên server lâu)
- ✅ Validation dữ liệu đầu vào

---

## 🔧 Cấu Hình

### **Program.cs**
```csharp
// Đã thêm
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICartService, CartService>();
```

### **AppDbContext.cs**
```csharp
public DbSet<Order> Orders { get; set; }
```

---

## 🧪 Test Cases

### **Test 1: Thêm vé vào giỏ**
1. ✅ Đăng nhập
2. ✅ Chọn vé
3. ✅ Giỏ hàng tăng số lượng
4. ✅ Tổng tiền tính đúng

### **Test 2: Cập nhật giỏ**
1. ✅ Thay đổi số lượng
2. ✅ Tổng tiền cập nhật
3. ✅ Xóa item khỏi giỏ
4. ✅ Tổng item giảm

### **Test 3: Thanh toán**
1. ✅ Chọn phương thức
2. ✅ Nhập thông tin
3. ✅ Xác nhận = Lưu Order
4. ✅ Giỏ hàng được xóa
5. ✅ Hiển thị xác nhận

### **Test 4: Lịch sử đơn**
1. ✅ Xem tất cả đơn
2. ✅ Xem chi tiết đơn
3. ✅ Hủy đơn (nếu Confirmed)
4. ✅ Status thay đổi

---

## 📱 Responsive Design

✅ **Hỗ trợ:**
- ✅ Desktop (1920px)
- ✅ Tablet (768px)
- ✅ Mobile (360px)

---

## ⚙️ Troubleshooting

| Vấn Đề | Nguyên Nhân | Giải Pháp |
|--------|-----------|----------|
| Giỏ hàng trống sau đăng nhập lại | Session được xóa | Bình thường - Session là tạm thời |
| Không thể thanh toán | Chưa đăng nhập | Đăng nhập trước |
| Lỗi tính tiền | Cơ sở dữ liệu | Check decimal precision |
| Không xem được đơn cũ | User khác | Chỉ xem được đơn của chính mình |

---

## 🎉 Tính Năng Sắp Tới

- 📧 Email confirmation
- 📸 Invoice/Receipt PDF
- 💳 Tích hợp gateway thanh toán
- 📊 Dashboard thống kê
- 🎁 Mã voucher/discount
- ⭐ Review và rating

---

**Hệ Thống Giỏ Hàng & Thanh Toán Hoàn Thành! ✅**
