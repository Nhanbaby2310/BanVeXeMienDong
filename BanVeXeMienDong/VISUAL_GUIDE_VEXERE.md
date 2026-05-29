# 🎨 Visual Guide - Vexere-Style Website

## **1. Trang Chủ (Home Page) - Autocomplete Search**

```
┌─────────────────────────────────────────────────────────────┐
│                    🌟 FLASH SALE 50% OFF 🌟                 │
├─────────────────────────────────────────────────────────────┤
│  🚌 Bán Vé Xe Miền Đông                                     │
│                                                              │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ Hà No        │  │ TP HCM       │  │ 10/04/2026   │      │
│  │ ▼ Hà Nội     │  │ ▼ TP.HCM     │  │              │      │
│  │  Hà Giang    │  │  Hải Phòng   │  └──────────────┘      │
│  │  Hải Phòng   │  │              │  [  Tìm Kiếm  ]       │
│  └──────────────┘  └──────────────┘                        │
│                                                              │
│  ✅ Chắc chắn có chỗ                                        │
│  ☎ Hỗ trợ 24/7                                             │
│  ⭐ Nhiều ưu đãi                                            │
│  💳 Thanh toán đa dạng                                     │
└─────────────────────────────────────────────────────────────┘
```

### **Autocomplete in Action:**
```
User types: "H"
↓
Suggestions appear:
- Hà Nội
- Hà Giang
- Hải Phòng
- Hưng Yên

User clicks: "Hà Nội"
↓
Input filled: "Hà Nội"
```

---

## **2. Trang Kết Quả Tìm Kiếm (Search Results)**

```
┌─────────────────────────────────────────────────────────────────────────┐
│  🚌 Bán Vé Xe Miền Đông                              [← Quay Lại]      │
├─────────────────────────────────────────────────────────────────────────┤
│  Điểm đi: [Hà Nội ▼] | Điểm đến: [TP.HCM ▼] | Ngày: [10/04/2026] [TK] │
├─────────────────┬─────────────────────────────────────────────────────┤
│                 │                                                       │
│  SIDEBAR        │  ⚡ FLASH SALE: Giảm đến 50%                         │
│  ────────       │                                                       │
│  Sắp Xếp        │  Kết quả: 323 chuyến xe                             │
│  ○ Mặc định     │  [Mặc định] [Giá thấp] [Giá cao] [Sớm nhất]        │
│  ○ Giờ sớm      │                                                       │
│  ○ Giá thấp     │  ┌─────────────────────────────────────────────────┐
│  ○ Rating cao   │  │ 🚌  │  Văn Minh Limousine  ⭐4.9 (244)         │
│                 │  │     │  Limousine giường nằm 38 chỗ               │
│  Giá Vé         │  │     │  09:45 → 14:25  (4h40m)                   │
│  [0───────Max]  │  │     │  📶WiFi  🚽Toilet  💧Nước                 │
│                 │  │     │                      350.000đ  Còn 26 chỗ │
│  Loại Xe        │  │     │                      [Chọn Chuyến]        │
│  ☑ Limousine    │  └─────────────────────────────────────────────────┘
│  ☐ Giường       │
│  ☐ Thường       │  ┌─────────────────────────────────────────────────┐
│                 │  │ 🚌  │  Thơm Phụng Limousine ⭐4.6 (25)          │
│  Tiện Ích       │  │     │  Limousine 24 phòng                        │
│  ☑ WiFi         │  │     │  21:30 → 01:00  (3h30m)                   │
│  ☐ Toilet       │  │     │  📶WiFi  💧Nước                           │
│  ☐ Nước         │  │     │                      Từ 250.000đ  Còn 16  │
│  ☐ Đồ ăn        │  │     │                      [Chọn Chuyến]        │
│                 │  └─────────────────────────────────────────────────┘
│  Điểm Đón       │
│  ○ Trung tâm    │  ┌─────────────────────────────────────────────────┐
│  ○ Sân bay      │  │ 🚌  │  Sao Nghệ Limousine   ⭐4.7 (1286)        │
│  ○ Bến xe       │  │     │  Limousine 22 giường VIP (Có WC)           │
│                 │  │     │  15:45 → 22:15  (6h30m)                   │
│  Đánh Giá       │  │     │  📶WiFi  🚽Toilet  💧Nước  🍔Đồ ăn      │
│  ★★★★★ 4.5+    │  │     │                      500.000đ  Còn 14 chỗ │
│  ★★★★☆ 4.0-4.5 │  │     │                      [Chọn Chuyến]        │
│  ★★★☆☆ 3.5-4.0 │  └─────────────────────────────────────────────────┘
│                 │
│                 │  ... (more cards)
│                 │
└─────────────────┴─────────────────────────────────────────────────────┘
```

---

## **3. Result Card - Chi Tiết**

```
┌────────────────────────────────────────────────────────────────┐
│  [🚌 IMAGE]  │  Company Name ⭐4.5 (244)                       │
│              │  Limousine Giường Nằm 38 Chỗ                    │
│              │                                                  │
│              │  09:45              14:25                       │
│              │  Hà Nội             TP.HCM                      │
│              │  (4h40m)            ← Duration                 │
│              │                                                  │
│              │  📶WiFi  🚽Toilet  💧Nước uống                 │
│              │  350.000đ                       Còn 26 chỗ     │
│              │                   [Chọn Chuyến]                │
└────────────────────────────────────────────────────────────────┘
```

**Elements:**
- 🚌 **Image**: Logo or bus icon
- ⭐ **Rating**: 4.5 stars + (244) reviews
- 📝 **Bus Type**: "Limousine Giường Nằm 38 Chỗ"
- 🕐 **Time**: Departure & Arrival time
- ⏱ **Duration**: Travel time
- 🏷️ **Amenities**: WiFi, Toilet, Water, Food tags
- 💰 **Price**: Large, prominent display
- 🪑 **Seats**: "Còn 26 chỗ"
- 🔘 **Button**: "Chọn Chuyến" - Yellow, clickable

---

## **4. Responsive Breakpoints**

### **Desktop (1200px+)**
```
┌─────────────────────────────────────────────┐
│  SIDEBAR (280px) │  RESULTS (1fr)           │
├──────────────────┼──────────────────────────┤
│ • Sắp Xếp        │ ☰ Toolbar (Inline sort) │
│ • Giá Vé         │ [Card] [Card] [Card]    │
│ • Loại Xe        │ [Card] [Card] [Card]    │
│ • Tiện Ích       │ [Card] [Card] [Card]    │
│ • Điểm Đón       │                         │
│ • Đánh Giá       │                         │
└──────────────────┴──────────────────────────┘
```

### **Tablet (768px-1024px)**
```
┌──────────────────────────────────────────────┐
│  [≡] SIDEBAR (Collapsible)  │  RESULTS     │
├─────────────────────────────┼──────────────┤
│ ☰ Toolbar                                    │
│ [Card] [Card]                                │
│ [Card] [Card]                                │
│ [Card] [Card]                                │
└──────────────────────────────────────────────┘
```

### **Mobile (< 768px)**
```
┌──────────────────────────────┐
│  [≡] SIDEBAR (Hidden)        │
├──────────────────────────────┤
│  ☰ Toolbar (Stacked)         │
├──────────────────────────────┤
│  [       Card Full Width  ]  │
│  [       Card Full Width  ]  │
│  [       Card Full Width  ]  │
└──────────────────────────────┘
```

---

## **5. Color Scheme**

```
Primary:     #2563eb (Blue)     - Sidebar, headers, tags
Secondary:   #fbbf24 (Yellow)   - Buttons, CTAs
Accent:      #ef4444 (Red)      - Promotions, alerts
Text Dark:   #1f2937 (Gray)     - Main text
Text Light:  #6b7280 (Gray)     - Descriptions
Background:  #f5f5f5 (Light)    - Page background
Card BG:     #ffffff (White)    - Cards
```

---

## **6. Typography**

```
Headers (H1, H2):      Font-weight 700-800, size 24-32px
Section Titles:        Font-weight 700, size 18-24px
Card Titles:           Font-weight 700, size 16px
Labels:                Font-weight 600, size 13px
Body Text:             Font-weight 400, size 14px
Small Text:            Font-weight 500, size 12px
```

---

## **7. Interactive States**

### **Button States**
```
Default:  Background #fbbf24 (Yellow)
Hover:    Background #f59e0b (Darker yellow)
Active:   Background #dc2626 (Red)
Disabled: Opacity 0.5, pointer-events none
```

### **Input States**
```
Default:   Border #e5e7eb, BG #f9fafb
Focus:     Border #2563eb, BG white, Shadow
Filled:    Border #d1d5db
Error:     Border #dc2626, BG #fee2e2
```

---

## **8. Animation & Transitions**

```
Default:       0.3s ease
Button Hover:  0.3s ease + translateY(-2px)
Filter Toggle: 0.3s ease (rotate expand icon)
Sort Active:   0.3s ease (background change)
```

---

## **9. Example User Journey**

```
1. HOME PAGE
   User: Autocomplete "Hà Nội" → Click "Hà Nội"
   Input filled with "Hà Nội"
   
2. SEARCH RESULTS
   Page: Shows 323 chuyến xe
   Sidebar: Ready to filter
   
3. FILTER (Optional)
   User: Selects "Giá < 300.000đ"
   Results: Updates to 45 chuyến xe
   
4. SORT (Optional)
   User: Clicks "Giá thấp"
   Results: Sorted by price ascending
   
5. SELECT TRIP
   User: Clicks "Chọn Chuyến"
   Page: Navigates to bus class selection
   
6. COMPLETE BOOKING
   ... (Continue in next steps)
```

---

## **10. Success Metrics**

✅ **Visual Design**
- [x] Matches Vexere color scheme
- [x] Responsive on all devices
- [x] Readable typography
- [x] Proper spacing & layout

✅ **User Experience**
- [x] Autocomplete works
- [x] Filters functional
- [x] Sort options work
- [x] Navigation clear
- [x] Buttons clickable

✅ **Performance**
- [x] CSS optimized
- [x] No layout shifts
- [x] Smooth animations
- [x] Fast loading

---

**Status**: ✅ **COMPLETE** - Ready for production!
