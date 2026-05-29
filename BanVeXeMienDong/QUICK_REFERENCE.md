# 🚀 QUICK REFERENCE - CHEAT SHEET

## ⚡ QUICK START

```bash
# Build project
dotnet build

# Run application
dotnet run

# Default URL: https://localhost:7059
```

---

## 🗺️ ROUTE MAP

### Public Routes (Không cần login)
```
GET  /                          → Home/Index (Trang chủ)
GET  /Ticket/SelectRoute        → Chọn tuyến đường
GET  /Ticket/Index              → Danh sách vé
GET  /Account/Register          → Đăng ký
GET  /Account/Login             → Đăng nhập
```

### Protected Routes (Cần login: User/Admin)
```
GET  /Ticket/SelectDateTime     → Chọn ngày
POST /Ticket/SelectBusClass     → Chọn hạng
GET  /Ticket/Create             → Chọn ghế
POST /Ticket/Create             → Tạo vé
GET  /Cart/Index                → Xem giỏ
POST /Cart/RemoveFromCart       → Xóa vé
GET  /Checkout/Index            → Thanh toán
POST /Checkout/ProcessPayment   → Xử lý thanh toán
GET  /Checkout/MyOrders         → Lịch sử đơn
```

### Admin Routes (Cần Admin role)
```
GET  /Admin/Index               → Dashboard
GET  /Admin/Users               → Quản lý users
GET  /Admin/EditUser/:id        → Sửa user
GET  /Admin/Tickets             → Quản lý vé
GET  /Admin/EditTicketPrice     → Sửa giá
```

### APIs (JSON)
```
GET  /Ticket/GetAvailableCities → Cities list
GET  /Ticket/GetBookedSeats     → Booked seats
POST /Cart/UpdateQuantity       → Update quantity
```

---

## 👥 DEFAULT USERS

### Admin
```
Username: admin
Password: 123456
Role: Admin
```

### Regular User
```
Username: user
Password: 123456
Role: User
```

---

## 💾 DATABASE TABLES

### Users
```
Id | Username | Password (Hash) | Email | Role | CreatedDate
```

### Tickets
```
Id | DiemDi | DiemDen | NgayDi | SoGhe | GiaVe | HangXe
```

### Orders
```
Id | UserId | OrderCode | OrderDate | TotalAmount | Status | ItemsJson | PaymentMethod | PhoneNumber | Email
```

---

## 🔑 SESSION KEYS

```csharp
HttpContext.Session.GetString("user")       // Username
HttpContext.Session.GetInt32("userId")      // User ID
HttpContext.Session.GetString("userRole")   // Role: "Admin" or "User"
```

---

## 📁 PROJECT STRUCTURE

```
BanVeXeMienDong/
├── Controllers/
│   ├── TicketController.cs         ← Ticket operations
│   ├── CartController.cs           ← Cart operations
│   ├── CheckoutController.cs       ← Payment processing
│   ├── AccountController.cs        ← Auth (Register/Login)
│   └── AdminController.cs          ← Admin operations
├── Models/
│   ├── User.cs                     ← User model
│   ├── Ticket.cs                   ← Ticket model
│   ├── Order.cs                    ← Order model
│   ├── CartItem.cs                 ← Cart item
│   ├── BusInfo.cs                  ← Bus layout
│   └── BusType.cs                  ← Bus types
├── Services/
│   └── CartService.cs              ← Cart business logic
├── Repositories/
│   ├── ITicketRepository.cs        ← Interface
│   └── MockTicketRepository.cs     ← Implementation
├── Data/
│   └── AppDbContext.cs             ← DbContext
├── Attributes/
│   └── AuthorizeAttribute.cs       ← Custom auth
├── Views/
│   ├── Home/Index.cshtml           ← Home page
│   ├── Ticket/                     ← Ticket views
│   ├── Cart/Index.cshtml           ← Cart view
│   ├── Checkout/                   ← Payment views
│   ├── Account/                    ← Auth views
│   └── Admin/                      ← Admin views
├── wwwroot/
│   ├── css/                        ← CSS files
│   ├── js/                         ← JavaScript
│   └── images/routes/              ← SVG images
├── Program.cs                      ← Configuration
├── appsettings.json               ← Settings
└── BanVeXeMienDong.csproj         ← Project file
```

---

## 🎨 CSS FILES

```
home-modern.css          → Home page styling
navbar-modern.css        → Navigation bar
style-modern.css         → Global styles
animations.css           → Transitions & animations
seats.css                → Bus seat layout
bus-class.css            → Bus type cards
search-results-vexere.css → Search results
```

---

## 🖼️ SVG ROUTE IMAGES

```
hue-sai-gon.svg          → Orange bus (Hà Nội - Đà Nẵng)
sai-gon-can-tho.svg      → Red bus (Sài Gòn - Cần Thơ)
ha-noi-hai-phong.svg     → Purple bus (Hà Nội - Hải Phòng)
da-nang-hcm.svg          → Green bus (Đà Nẵng - TP.HCM)
```

All: 400×250, viewBox format

---

## 🔧 KEY CLASSES

### CartService
```csharp
public void AddToCart(CartItem item)      // Add
public void RemoveFromCart(int itemId)    // Remove
public decimal GetTotal()                 // Sum
public List<CartItem> GetCart()           // Get all
public void ClearCart()                   // Clear
```

### TicketController
```csharp
public IActionResult Index()              // List
public IActionResult SelectRoute()        // Choose route
public IActionResult SelectDateTime()     // Choose date
public IActionResult SelectBusClass()     // Choose class
public IActionResult Create()             // Choose seat
public IActionResult GetAvailableCities() // API
```

### AuthorizeAttribute
```csharp
[Authorize("Admin")]           // Require Admin
[Authorize("Admin", "User")]   // Require Admin or User
```

---

## 📝 COMMON OPERATIONS

### Add to Cart
```csharp
var item = new CartItem { ... };
_cartService.AddToCart(item);
```

### Get Cart Total
```csharp
decimal total = _cartService.GetTotal();
```

### Check Authorization
```csharp
var role = HttpContext.Session.GetString("userRole");
if (role == "Admin") { ... }
```

### Save to Session
```csharp
HttpContext.Session.SetString("user", username);
HttpContext.Session.SetInt32("userId", userId);
```

### Get from Session
```csharp
var user = HttpContext.Session.GetString("user");
var id = HttpContext.Session.GetInt32("userId");
```

### Hash Password
```csharp
private string Hash(string password)
{
    using var sha = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(password);
    var hash = sha.ComputeHash(bytes);
    return Convert.ToBase64String(hash);
}
```

### Redirect with TempData
```csharp
TempData["Success"] = "✅ Done!";
return RedirectToAction("Index");
```

---

## 🐛 DEBUGGING TIPS

### Check Session
```csharp
var user = HttpContext.Session.GetString("user");
Console.WriteLine($"Current user: {user}");
```

### Check Cart
```csharp
var cart = _cartService.GetCart();
Console.WriteLine($"Cart items: {cart.Count}");
```

### Check Database
```csharp
using (var context = new AppDbContext())
{
    var users = context.Users.ToList();
}
```

### Browser DevTools
```
F12 → Network tab → Check API responses
F12 → Application → Cookies → Check session
F12 → Console → Check JS errors
```

---

## ⚙️ CONFIGURATION

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=BanVeXeMienDong;Trusted_Connection=true;"
  }
}
```

### Program.cs Services
```csharp
builder.Services.AddDbContext<AppDbContext>(...)
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); })
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ITicketRepository, MockTicketRepository>();
```

### Middleware Order
```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();           // ← MUST be before MapControllerRoute
app.UseAuthorization();
app.MapControllerRoute(...);
```

---

## 🚦 FLOW DIAGRAMS

### Ticket Purchase Flow
```
Home Page
  ↓
SelectRoute (Choose route)
  ↓
SelectDateTime (Choose date)
  ↓
SelectBusClass (Standard/Premium)
  ↓
Create (Choose seats 1-40)
  ↓
AddToCart (Save to session)
  ↓
Cart/Index (View cart)
  ↓
Checkout/Index (Confirm)
  ↓
ProcessPayment (Create Order)
  ↓
OrderConfirmation (Success)
```

### Authentication Flow
```
Register → Hash password → Save to DB
  ↓
Login → Check username → Verify hash → Set session
  ↓
Protected routes → Check session role → Allow/Deny
```

---

## 📊 QUICK STATUS CHECK

```bash
# Check build
dotnet build                    # Should: Build successful

# Check database
dotnet ef database update       # Should: Updates applied

# Check running
dotnet run                      # Should: Listening on HTTPS
```

---

## 🎯 COMMON ISSUES & QUICK FIXES

| Issue | Fix |
|-------|-----|
| Session null | Check `app.UseSession()` order |
| Cart empty | Check JSON serialization options |
| Login fail | Check password hash algorithm |
| Authorize fail | Check custom attribute on controller |
| Image 404 | Check SVG files in wwwroot/images/routes/ |
| Database error | Check connection string in appsettings.json |
| API 404 | Check [HttpGet] attribute |

---

## 💡 TIPS & TRICKS

1. **View Cart Contents** (Razor):
   ```html
   @if (Context.Session.GetString("user") != null)
   {
       <span>Hello, @Context.Session.GetString("user")</span>
   }
   ```

2. **Redirect with Message**:
   ```csharp
   TempData["Success"] = "✅ Action completed";
   return RedirectToAction("Index");
   ```

3. **Debug in View**:
   ```html
   @{
       var cart = ViewBag.Cart;
       // Breakpoint here
   }
   ```

4. **Check Authorization**:
   ```csharp
   var role = HttpContext.Session.GetString("userRole");
   if (role != "Admin") return Unauthorized();
   ```

---

## 📞 SUPPORT FILES

- `FINAL_SYSTEM_CHECK_REPORT.md` - Full status report
- `FUNCTIONALITY_CHECK_REPORT.md` - Features detail
- `FULL_TESTING_GUIDE.md` - 25+ test cases
- `ISSUES_AND_FIXES.md` - Problems & solutions

---

**Last Updated**: 2025-01-09  
**Status**: ✅ ALL SYSTEMS OPERATIONAL

