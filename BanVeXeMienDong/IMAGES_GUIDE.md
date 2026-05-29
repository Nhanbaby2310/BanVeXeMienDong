# 📸 Hướng Dẫn Thêm Hình Ảnh Vào Website

## 📁 Cấu Trúc Thư Mục

Tạo thư mục sau trong project:

```
BanVeXeMienDong/
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── lib/
│   └── images/                    ← Tạo thư mục này
│       ├── routes/                ← Hình ảnh tuyến đường
│       │   ├── sai-gon.jpg
│       │   ├── ha-noi.jpg
│       │   ├── da-nang.jpg
│       │   └── hue.jpg
│       └── app-mockup.png         ← Hình ảnh ứng dụng
```

## 🖼️ Hình Ảnh Cần Thêm

### 1. **Hình Ảnh Tuyến Đường** (4 ảnh)
   - **Vị trí**: `/wwwroot/images/routes/`
   - **Tên file**: 
     - `sai-gon.jpg` (Sài Gòn → Cần Thơ)
     - `ha-noi.jpg` (Hà Nội → Hải Phòng)
     - `da-nang.jpg` (Đà Nẵng → TP.HCM)
     - `hue.jpg` (Huế → Sài Gòn)
   - **Kích thước**: 300x200px (aspect ratio 3:2)
   - **Format**: JPG/PNG
   - **Dung lượng**: 30-50KB mỗi ảnh

### 2. **Hình Ảnh Ứng Dụng** (1 ảnh)
   - **Vị trí**: `/wwwroot/images/`
   - **Tên file**: `app-mockup.png`
   - **Kích thước**: 250x400px (aspect ratio 9:16)
   - **Format**: PNG (để trong suốt)
   - **Dung lượng**: 50-100KB

## 💡 Nguồn Hình Ảnh Miễn Phí

### Tuyến Đường (Thành phố, Điểm du lịch)
- **Unsplash**: https://unsplash.com/
- **Pexels**: https://www.pexels.com/
- **Pixabay**: https://pixabay.com/
- **Freepik**: https://www.freepik.com/

**Tìm kiếm**: 
- "Ho Chi Minh City" (Sài Gòn)
- "Hanoi city" (Hà Nội)
- "Da Nang beach" (Đà Nẵng)
- "Hue city" (Huế)

### App Mockup
- **Mockup Generator**: https://www.mockuper.com/
- **ScreenMock**: https://screenmock.com/
- **iMockups**: https://www.imockups.com/
- **Figma Templates**: https://www.figma.com/

## 🚀 Cách Thêm Hình Ảnh

### **Cách 1: Dùng Visual Studio**
1. Mở Solution Explorer
2. Chuột phải vào `wwwroot` → New Folder → Đặt tên `images`
3. Chuột phải vào `images` → New Folder → Đặt tên `routes`
4. Drag & drop ảnh vào thư mục tương ứng

### **Cách 2: Dùng File Explorer**
1. Mở File Explorer
2. Điều hướng tới: `C:\Users\YourUsername\source\repos\BanVeXeMienDong\wwwroot\`
3. Tạo thư mục `images` (nếu chưa có)
4. Tạo thư mục con `routes` trong `images`
5. Copy hình ảnh vào thư mục tương ứng

### **Cách 3: Dùng Terminal**
```powershell
# Tạo thư mục
mkdir wwwroot\images
mkdir wwwroot\images\routes

# Copy ảnh từ nơi khác (ví dụ: Downloads)
Copy-Item "C:\Users\YourUsername\Downloads\sai-gon.jpg" "wwwroot\images\routes\"
```

## 📋 Danh Sách File Cần Thêm

| Thứ tự | File Name | Kích thước | Nơi lưu |
|--------|-----------|-----------|---------|
| 1 | sai-gon.jpg | 300x200 | `/images/routes/` |
| 2 | ha-noi.jpg | 300x200 | `/images/routes/` |
| 3 | da-nang.jpg | 300x200 | `/images/routes/` |
| 4 | hue.jpg | 300x200 | `/images/routes/` |
| 5 | app-mockup.png | 250x400 | `/images/` |

## ✅ Kiểm Tra Sau Khi Thêm

1. **Rebuild Project**
   ```powershell
   dotnet build
   ```

2. **Kiểm tra trong browser**
   - Mở http://localhost:5000/ (hoặc port của bạn)
   - Scroll down đến "Tuyến Đường Phổ Biến"
   - Hình ảnh phải hiển thị

3. **Nếu hình ảnh không hiển thị**
   - Kiểm tra đường dẫn file
   - Kiểm tra tên file (phải đúng chính xác, có phân biệt hoa/thường)
   - Clear browser cache (Ctrl+F5)

## 🎨 Optimization Tips

### Compress Hình Ảnh
Dùng công cụ như:
- **TinyPNG**: https://tinypng.com/
- **ImageOptim**: https://imageoptim.com/
- **FileOptimizer**: https://nikkhokkho.sourceforge.io/

### Responsive Images
CSS đã được cấu hình tự động scale hình ảnh theo kích thước màn hình.

### Lazy Loading (Tùy chọn)
Thêm `loading="lazy"` vào thẻ `<img>`:
```html
<img src="/images/routes/sai-gon.jpg" alt="Sài Gòn" loading="lazy">
```

## 📐 Khuyến Nghị Kích Thước

### Route Images
- **Desktop**: 300x200px
- **Mobile**: 200x150px (CSS tự scale)
- **DPI**: 72 DPI
- **Format**: JPG (chất lượng 80-90%)

### App Mockup
- **Kích thước**: 250x400px
- **Format**: PNG (transparent background)
- **DPI**: 72 DPI

## ⚠️ Lưu Ý

1. **Tên file**: Dùng chữ thường, không dấu
   - ✅ Đúng: `sai-gon.jpg`
   - ❌ Sai: `Sài Gòn.jpg` hoặc `Sai Gon.jpg`

2. **Đường dẫn**: 
   - ✅ Đúng: `/images/routes/sai-gon.jpg`
   - ❌ Sai: `images/routes/sai-gon.jpg` hoặc `../images/routes/sai-gon.jpg`

3. **Format**:
   - ✅ JPG cho ảnh động, PNG cho logo/icon
   - ❌ BMP, GIF (chậm), WEBP (không hỗ trợ tốt)

4. **Dung lượng**:
   - ✅ < 100KB mỗi ảnh
   - ❌ > 500KB (làm chậm trang)

## 🔧 Code Hiện Tại

Trang web đã cập nhật để dùng đường dẫn hình ảnh:
```html
<!-- Route Images -->
<img src="/images/routes/sai-gon.jpg" alt="Sài Gòn" 
     onerror="this.src='https://via.placeholder.com/300x200?text=Sài+Gòn'">

<!-- App Mockup -->
<img src="/images/app-mockup.png" alt="App Mockup"
     onerror="this.src='https://via.placeholder.com/250x400?text=App'">
```

**Lưu ý**: `onerror` sẽ tự động dùng placeholder nếu hình ảnh không tìm thấy.

## 📞 Troubleshooting

### Vấn đề: Hình ảnh không hiển thị
**Giải pháp**:
1. Kiểm tra đường dẫn file
2. Kiểm tra tên file (case-sensitive)
3. Đảm bảo file nằm trong `/wwwroot/`
4. Rebuild project
5. Clear browser cache (Ctrl+Shift+Delete)

### Vấn đề: Hình ảnh mờ hoặc bị căng
**Giải pháp**:
1. Tăng DPI lên 96
2. Sử dụng ảnh chất lượng cao hơn
3. Kiểm tra CSS `object-fit: cover` (đã được thiết lập)

### Vấn đề: Load chậm
**Giải pháp**:
1. Compress hình ảnh
2. Sử dụng JPG thay vì PNG
3. Giảm kích thước file

## ✨ Tính Năng Tự Động

- ✅ Fallback to placeholder nếu ảnh không tìm thấy
- ✅ Responsive (tự scale theo màn hình)
- ✅ Hover effect (zoom ảnh)
- ✅ Lazy loading ready (nếu thêm `loading="lazy"`)

---

**Sau khi thêm ảnh, rebuild và test lại website!** 🚀
