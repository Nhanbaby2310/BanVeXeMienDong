# ⚡ Quick Start - Vexere Website

## **🎯 What Was Changed?**

Your bus ticket website now looks and works like **Vexere** (Vietnamese bus booking platform).

### **Before vs After**

| Feature | Before | After |
|---------|--------|-------|
| Search | Plain text inputs | **Smart autocomplete** ✨ |
| Results | Simple list | **Vexere-style cards** 🎨 |
| Filters | None | **Advanced sidebar filters** 📊 |
| Rating | Not shown | **Star ratings + reviews** ⭐ |
| Amenities | Not shown | **WiFi, Toilet, Water tags** 🏷️ |
| Design | Basic | **Professional & modern** 💎 |
| Mobile | Poor | **Fully responsive** 📱 |

---

## **🚀 How to Use (For Users)**

### **Step 1: Go to Home Page**
```
URL: http://localhost:5000/
or http://yoursite.com/
```

### **Step 2: Use Smart Search**
```
1. Click "Nơi xuất phát" (Departure)
   → Type "H" 
   → See suggestions: Hà Nội, Hà Giang, Hải Phòng
   → Click "Hà Nội"

2. Click "Nơi đến" (Destination)
   → Type "TP"
   → See suggestion: TP.HCM
   → Click TP.HCM

3. Select date: 10/04/2026

4. Click "Tìm Kiếm" (Search)
```

### **Step 3: Browse Results**
```
Page shows: 323 chuyến xe

Each card shows:
- 🚌 Bus company name + rating
- 📊 Bus type (Limousine, Giường, etc.)
- 🕐 Departure & arrival time
- ⏱️ Travel duration
- 🏷️ Amenities (WiFi, Toilet, Water, Food)
- 💰 Price
- 🪑 Available seats
- 🔘 "Chọn Chuyến" button
```

### **Step 4: Filter (Optional)**
```
Sidebar on left:
- Sort by: Default, Price, Time, Rating
- Filter by: Price range, Bus type, Amenities, Pickup point
- Click to expand/collapse each section
```

### **Step 5: Choose Trip**
```
Click "Chọn Chuyến" (Choose Trip)
→ Navigate to seat selection
→ Complete booking
```

---

## **👨‍💻 How to Customize (For Developers)**

### **Change Colors**

Open: `wwwroot/css/search-results-vexere.css`

```css
/* Change primary color from blue to green */
.vexere-header {
    background: linear-gradient(135deg, #10b981 0%, #059669 100%);
}

/* Change button color from yellow to purple */
.choose-btn {
    background: #8b5cf6;
}

/* Change accent color from red to orange */
.promotion-banner {
    background: linear-gradient(135deg, #f97316 0%, #ea580c 100%);
}
```

### **Change Layout**

```css
/* Make sidebar wider */
.search-results-container {
    grid-template-columns: 350px 1fr; /* Changed from 280px */
}

/* Increase card spacing */
.results-section {
    gap: 30px; /* Changed from 20px */
}

/* Change sidebar width for mobile */
@media (max-width: 768px) {
    .search-results-container {
        grid-template-columns: 1fr; /* Full width */
    }
}
```

### **Add New Filter**

Edit: `Views/Ticket/SelectRoute.cshtml`

Find the "Sidebar Filter" section and add:

```html
<div class="filter-section">
    <div class="filter-title expandable" onclick="toggleFilter(this)">
        Nhà Xe
        <span class="expand-icon expanded">▼</span>
    </div>
    <div class="filter-options">
        <label class="filter-option">
            <input type="checkbox" value="vxmd">
            <span>Vé Xe Miền Đông</span>
            <span class="count">45</span>
        </label>
        <label class="filter-option">
            <input type="checkbox" value="vanminh">
            <span>Văn Minh Limousine</span>
            <span class="count">28</span>
        </label>
    </div>
</div>
```

### **Change Amenities Tags**

Find in `SelectRoute.cshtml`:

```html
<div class="amenities">
    <span class="amenity-tag">📶 WiFi</span>
    <span class="amenity-tag">🚽 Toilet</span>
    <span class="amenity-tag highlight">💧 Nước</span>
</div>
```

Change to add/remove tags as needed.

---

## **📁 File Locations**

### **Frontend Files**
```
Views/
├── Home/
│   └── Index.cshtml          (Home page with search)
└── Ticket/
    └── SelectRoute.cshtml    (Search results page)

wwwroot/css/
├── home-modern.css           (Home page styles)
└── search-results-vexere.css (Results page styles)
```

### **Backend Files**
```
TicketController/
└── TicketController.cs       (API: GetAvailableCities)
```

---

## **🔍 API Endpoints**

### **Get Available Cities**
```
GET /Ticket/GetAvailableCities?search=Ha

Response:
["Hà Nội", "Hà Giang", "Hải Phòng"]
```

Usage in JavaScript:
```javascript
fetch('/Ticket/GetAvailableCities?search=Ha')
  .then(res => res.json())
  .then(cities => console.log(cities))
```

---

## **🎨 Color Reference**

```
Primary Blue:     #2563eb
Secondary Yellow: #fbbf24
Accent Red:       #ef4444
Text Dark:        #1f2937
Text Light:       #6b7280
Text Lighter:     #9ca3af
Background:       #f5f5f5
White:            #ffffff
```

---

## **📱 Responsive Breakpoints**

```
Desktop:  1200px and up  → Full layout with sidebar
Tablet:   768px-1024px   → Sidebar collapses
Mobile:   Below 768px    → Full-width, no sidebar
```

---

## **🧪 Testing Checklist**

### **Desktop**
- [ ] Search autocomplete works
- [ ] Results display correctly
- [ ] Sidebar filters visible
- [ ] Cards show all info
- [ ] Buttons clickable
- [ ] Hover effects work

### **Mobile**
- [ ] Autocomplete works
- [ ] Full-width cards
- [ ] Buttons large enough
- [ ] No horizontal scroll
- [ ] Sidebar hidden
- [ ] Responsive images

### **Functionality**
- [ ] Search form submits
- [ ] Results filter correctly
- [ ] Sort works
- [ ] "Chọn Chuyến" navigates
- [ ] No console errors
- [ ] Forms validate

---

## **🚨 Troubleshooting**

### **Autocomplete not working?**
1. Check `GetAvailableCities` endpoint returns data
2. Open DevTools (F12) → Network tab
3. Search for `/Ticket/GetAvailableCities`
4. Check response is JSON array

### **Results page broken?**
1. Clear browser cache (Ctrl+Shift+Delete)
2. Hard refresh (Ctrl+Shift+R)
3. Check CSS file loads: `search-results-vexere.css`
4. Check browser console for JS errors

### **Layout broken on mobile?**
1. Check viewport meta tag in `_Layout.cshtml`:
   ```html
   <meta name="viewport" content="width=device-width, initial-scale=1.0">
   ```
2. Test in device emulation (F12 → Device mode)
3. Check media queries in CSS

### **Buttons don't navigate?**
1. Check form `action` attribute
2. Check controller route exists
3. Check form `method` is correct (POST/GET)
4. Check no JavaScript prevents form submission

---

## **📚 Learning Resources**

- **Vexere Website**: https://vexere.com/
- **CSS Grid**: https://css-tricks.com/snippets/css/complete-guide-grid/
- **Responsive Design**: https://developer.mozilla.org/en-US/docs/Learn/CSS/CSS_layout/Responsive_Design
- **Accessibility**: https://www.w3.org/WAI/fundamentals/

---

## **🎯 Next Steps**

After implementation:

1. **Add More Filters**
   - Bus operator/company
   - Departure time ranges
   - Seat type preferences

2. **Implement Backend Filtering**
   - Apply filters to actual data
   - Return filtered results

3. **Add More Features**
   - User reviews & ratings
   - Price alerts
   - Search history
   - Favorite routes

4. **Optimize Performance**
   - Add pagination
   - Lazy load images
   - Cache results

5. **Improve Mobile**
   - Add touch-friendly buttons
   - Optimize for slow networks
   - Add loading indicators

---

## **✅ Checklist**

- [x] Autocomplete search added
- [x] Vexere-style results page
- [x] Sidebar filters
- [x] Result cards with ratings
- [x] Responsive design
- [x] CSS styling
- [x] JavaScript functionality
- [ ] Backend filtering
- [ ] User reviews
- [ ] Price alerts

---

## **📞 Need Help?**

Check these files for more info:
- `VEXERE_IMPLEMENTATION_COMPLETE.md` - Full documentation
- `VEXERE_STYLE_SEARCH.md` - Feature guide
- `VISUAL_GUIDE_VEXERE.md` - Visual design guide

---

**Status**: ✅ **READY TO USE**

Build successful! 🎉

Your website now looks professional like Vexere!
