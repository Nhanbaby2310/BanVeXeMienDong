# 🔧 DANH SÁCH CÁC ISSUE CÓ THỂ VÀ CÁCH SỬA

## 📋 Các Issue Phổ Biến & Giải Pháp

---

## 1. ❌ "The name 'user' does not exist in current context" (Razor)

### ❌ Lỗi
```html
<!-- Index.cshtml -->
@if (user != null)  <!-- ❌ LỖI -->
```

### ✅ Sửa
```html
<!-- Phải dùng HttpContext.Session -->
@if (Context.Session.GetString("user") != null)
{
    <span>Xin chào, @Context.Session.GetString("user")</span>
}
```

---

## 2. ❌ "Session is null"

### ❌ Lỗi
Ở Program.cs:
```csharp
app.UseRouting();
app.MapControllerRoute(...);
app.UseSession();  // ❌ PHẢI TRỊ, KHÔNG CÓ HIỆU LỰC
```

### ✅ Sửa
```csharp
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();  // ✅ PHẢI ở ĐÂY
app.UseAuthorization();
app.MapControllerRoute(...);
```

---

## 3. ❌ "Cart items không save"

### ❌ Lỗi
```csharp
var cartJson = JsonSerializer.Serialize(cart);  // ❌ Mặc định camelCase
var deserialized = JsonSerializer.Deserialize<List<CartItem>>(cartJson);  // ❌ Case mismatch
```

### ✅ Sửa
```csharp
var options = new JsonSerializerOptions 
{ 
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

var cartJson = JsonSerializer.Serialize(cart, options);
var deserialized = JsonSerializer.Deserialize<List<CartItem>>(cartJson, options);
```

---

## 4. ❌ "IHttpContextAccessor is null"

### ❌ Lỗi
Ở CartService:
```csharp
public CartService(IHttpContextAccessor httpContextAccessor)
{
    _httpContextAccessor = httpContextAccessor;  // ❌ Null nếu không register
}
```

### ✅ Sửa
Ở Program.cs:
```csharp
builder.Services.AddHttpContextAccessor();  // ✅ PHẢI ADD
builder.Services.AddScoped<ICartService, CartService>();
```

---

## 5. ❌ "GetAvailableCities returns 404"

### ❌ Lỗi
Controller không có endpoint
```csharp
// ❌ Không có [HttpGet] GetAvailableCities
public IActionResult Index() { ... }
```

### ✅ Sửa
```csharp
[HttpGet]
public IActionResult GetAvailableCities(string search = "")
{
    var allCities = new HashSet<string>();
    // ... logic
    return Json(filteredCities);
}
```

---

## 6. ❌ "Authorize attribute not working"

### ❌ Lỗi
```csharp
[Authorize]  // ❌ Built-in, không hoạt động
public IActionResult Index() { }
```

### ✅ Sửa
Dùng custom attribute:
```csharp
[Authorize("Admin", "User")]  // ✅ Custom AuthorizeAttribute
public IActionResult Index() { }
```

và check AuthorizeAttribute.cs có:
```csharp
public class AuthorizeAttribute : Attribute { }
// ... implement logic
```

---

## 7. ❌ "Password không verify"

### ❌ Lỗi
```csharp
// Login
var storedHash = user.Password;  // "abc123hash"
var inputHash = Hash("password");  // "xyz789hash"
if (storedHash != inputHash) { ❌ LỖI }  // Luôn không match
```

### ✅ Sửa
```csharp
private string Hash(string password)
{
    using var sha = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(password);
    var hash = sha.ComputeHash(bytes);
    return Convert.ToBase64String(hash);  // ✅ Phải convert base64
}

// Register
user.Password = Hash(user.Password);  // ✅ Hash khi lưu
_context.Users.Add(user);

// Login
var storedHash = user.Password;
var inputHash = Hash(loginPassword);
if (storedHash == inputHash) { ✅ OK }
```

---

## 8. ❌ "Duplicate seat selection"

### ❌ Lỗi
```html
<!-- Chọn ghế A1, B1, A1 (trùng) -->
<!-- Form submit = SoGhe: "A1,B1,A1" -->
```

### ✅ Sửa
Ở TicketController.Create (POST):
```csharp
var seats = ticket.SoGhe.Split(',').Select(s => s.Trim()).Distinct().ToList();
if (seats.Count != ticket.SoGhe.Split(',').Length)
{
    ModelState.AddModelError("SoGhe", "Có ghế được chọn trùng lặp");
    return View(busInfo);
}
```

---

## 9. ❌ "Route card images không hiển thị"

### ❌ Lỗi
```html
<!-- Ở Index.cshtml -->
<img src="/images/routes/hue-sai-gon.svg" />  <!-- ❌ 404 Not Found -->
```

**Nguyên nhân**: File SVG không tồn tại hoặc path sai

### ✅ Sửa
1. Verify file tồn tại:
   ```
   wwwroot/images/routes/hue-sai-gon.svg
   ```

2. Check file có valid SVG format:
   ```xml
   <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 400 250">
     ...
   </svg>
   ```

3. Nếu file lỗi XML → Replace với SVG mới

---

## 10. ❌ "TempData lost between requests"

### ❌ Lỗi
```csharp
// SelectRoute
TempData["DiemDi"] = "Hà Nội";
return RedirectToAction("SelectDateTime");

// SelectDateTime
var diemDi = TempData["DiemDi"];  // ❌ Null, TempData expired
```

### ✅ Sửa
```csharp
// SelectRoute
TempData["DiemDi"] = "Hà Nội";
TempData.Keep("DiemDi");  // ✅ Keep for next request
return RedirectToAction("SelectDateTime");

// SelectDateTime
var diemDi = TempData["DiemDi"];  // ✅ OK
TempData.Keep("DiemDi");  // ✅ Keep lại nếu redirect tiếp
```

---

## 11. ❌ "Cart total calculation wrong"

### ❌ Lỗi
```csharp
public decimal GetTotal()
{
    var cart = GetCart();
    return cart.Sum(x => x.Price);  // ❌ Chỉ sum price, không × quantity
}
```

### ✅ Sửa
```csharp
public decimal GetTotal()
{
    var cart = GetCart();
    return cart.Sum(x => x.Price * x.Quantity);  // ✅ Nhân với số lượng
}
```

---

## 12. ❌ "Booked seats filter không hoạt động"

### ❌ Lỗi
```csharp
var bookedSeats = _repository.GetAll()
    .Where(t => t.NgayDi.Date == date.Date)
    .SelectMany(t => t.SoGhe.Split(',').Select(s => s.Trim()))
    .Distinct()
    .ToList();  // ❌ Lấy tất cả, không filter route + class
```

### ✅ Sửa
```csharp
var bookedSeats = _repository.GetAll()
    .Where(t => t.NgayDi.Date == date.Date 
        && t.HangXe == (BusClass)busClass  // ✅ Filter hạng
        && t.DiemDi == diemDi              // ✅ Filter tuyến
        && t.DiemDen == diemDen)
    .SelectMany(t => t.SoGhe.Split(',').Select(s => s.Trim()))
    .Distinct()
    .ToList();
```

---

## 13. ❌ "Database migration needed"

### ❌ Lỗi
```
InvalidOperationException: Unable to resolve service for type 'BanVeXeMienDong.Data.AppDbContext'
```

### ✅ Sửa
```bash
# Update database
dotnet ef database update
```

hoặc nếu DB context chưa config:

```csharp
// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

Và appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);Database=BanVeXeMienDong;Trusted_Connection=true;"
  }
}
```

---

## 14. ❌ "ViewBag value missing in view"

### ❌ Lỗi
```csharp
// Controller
ViewBag.Routes = routes;

// View
@ViewBag.Routes  // ❌ Null, không cast được
```

### ✅ Sửa
```csharp
// Controller
ViewBag.Routes = routes;
ViewBag.RouteCount = routes.Count();

// View
@if (ViewBag.Routes != null)
{
    @foreach (var route in ViewBag.Routes)
    {
        <p>@route.DiemDi → @route.DiemDen</p>
    }
}
```

---

## 15. ❌ "AddToCart quantity wrong"

### ❌ Lỗi
```csharp
var cartItem = new CartItem
{
    Quantity = 1  // ❌ Luôn = 1, không phải số ghế
};
```

### ✅ Sửa
```csharp
var numberOfSeats = ticket.SoGhe.Split(',').Length;
var cartItem = new CartItem
{
    Quantity = numberOfSeats  // ✅ = số ghế được chọn
};
```

---

## 🎯 Summary Fixes

| Issue | Root Cause | Fix |
|-------|-----------|-----|
| Session null | Order sai | Đổi thứ tự UseSession() |
| Cart null | JsonOptions | Add PropertyNameCaseInsensitive |
| Authorize fail | Missing attribute | Dùng custom Authorize |
| Password fail | No base64 | Convert.ToBase64String() |
| Images 404 | File không tồn tại | Create/Update SVG files |
| TempData lost | Không Keep | TempData.Keep() |
| Total wrong | Không × quantity | Nhân price * quantity |
| Booked wrong | Filter không đủ | Add HangXe, DiemDi, DiemDen |
| DB error | Not configured | AddDbContext + migration |
| ViewBag null | Không check null | if (ViewBag.X != null) |

---

## ✅ Verification Checklist

Sau khi fix, kiểm tra:
- [x] Build successful
- [x] Không có compile errors
- [x] Runtime không có exceptions
- [x] Session hoạt động
- [x] Cart lưu data
- [x] Images hiển thị
- [x] Authorization work
- [x] DB save data

