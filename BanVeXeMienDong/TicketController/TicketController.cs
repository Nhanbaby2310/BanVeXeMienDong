using BanVeXeMienDong.Models;
using BanVeXeMienDong.Repositories;
using BanVeXeMienDong.Attributes;
using BanVeXeMienDong.Services;
using Microsoft.AspNetCore.Mvc;

namespace BanVeXeMienDong.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _repository;
        private readonly ICartService _cartService;

        public TicketController(ITicketRepository repository, ICartService cartService)
        {
            _repository = repository;
            _cartService = cartService;
        }

        // 👉 KHÔNG CẦN ĐĂNG NHẬP - Xem danh sách vé khả dụng cho người dùng
        public IActionResult Index(string? diemDi = null, string? diemDen = null, DateTime? ngayDi = null, int page = 1)
        {
            const int pageSize = 15;
            var allTickets = _repository.GetAll();

            // Lọc theo điểm đi nếu có
            if (!string.IsNullOrEmpty(diemDi))
                allTickets = allTickets.Where(t => t.DiemDi == diemDi).ToList();

            // Lọc theo điểm đến nếu có
            if (!string.IsNullOrEmpty(diemDen))
                allTickets = allTickets.Where(t => t.DiemDen == diemDen).ToList();

            // Lọc theo ngày đi nếu có
            if (ngayDi.HasValue)
                allTickets = allTickets.Where(t => t.NgayDi.Date == ngayDi.Value.Date).ToList();

            // Sắp xếp theo ngày đi
            allTickets = allTickets.OrderBy(t => t.NgayDi).ToList();

            // Pagination
            var totalItems = allTickets.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));
            var pagedTickets = allTickets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

            // Lưu các filter vào ViewBag để hiển thị trong view
            ViewBag.DiemDi = diemDi;
            ViewBag.DiemDen = diemDen;
            ViewBag.NgayDi = ngayDi?.ToString("yyyy-MM-dd");

            // Lấy danh sách tuyến đường để hiển thị trong filter
            var routes = _repository.GetAll()
                .GroupBy(t => new { t.DiemDi, t.DiemDen })
                .Select(g => new { g.Key.DiemDi, g.Key.DiemDen })
                .ToList();

            ViewBag.Routes = routes;

            return View(pagedTickets);
        }

        // 📍 BƯỚC 1: Chọn tuyến đường trước hoặc xem kết quả nếu có parameters
        public IActionResult SelectRoute(string? diemDi = null, string? diemDen = null, DateTime? ngayDi = null)
        {
            // Nếu có query parameters, lọc kết quả ngay
            if (!string.IsNullOrEmpty(diemDi) && !string.IsNullOrEmpty(diemDen) && ngayDi.HasValue)
            {
                return Index(diemDi, diemDen, ngayDi);
            }

            // 👉 LẤY TUYẾN ĐƯỜNG TỪ DATABASE
            var dbRoutes = _repository.GetAll()
                .GroupBy(t => new { t.DiemDi, t.DiemDen })
                .Select(g => new { g.Key.DiemDi, g.Key.DiemDen })
                .Distinct()
                .ToList();

            // 👉 TẠO CÁC TUYẾN MẶC ĐỊNH NẾU CHƯA CÓ
            var defaultRoutes = new List<dynamic>
            {
                new { DiemDi = "Hà Nội", DiemDen = "HCM" },
                new { DiemDi = "Hà Nội", DiemDen = "Đà Nẵng" },
                new { DiemDi = "Hà Nội", DiemDen = "Hải Phòng" },
                new { DiemDi = "HCM", DiemDen = "Đà Nẵng" },
                new { DiemDi = "HCM", DiemDen = "Cần Thơ" },
                new { DiemDi = "Đà Nẵng", DiemDen = "Quy Nhơn" }
            };

            // 👉 MERGE: TẬP HỢP TỪ DB + DEFAULT ROUTES
            var allRoutes = dbRoutes.Cast<dynamic>().ToList();

            foreach (var defaultRoute in defaultRoutes)
            {
                if (!dbRoutes.Any(r => r.DiemDi == defaultRoute.DiemDi && r.DiemDen == defaultRoute.DiemDen))
                {
                    allRoutes.Add(defaultRoute);
                }
            }

            ViewBag.Routes = allRoutes;
            return View("SelectRoute");
        }

        // 📅 BƯỚC 2: Chọn ngày và giờ đi (sau khi chọn tuyến đường) - Chấp nhận GET & POST
        public IActionResult SelectDateTime(string diemDi = "", string diemDen = "")
        {
            // Nếu không có parameter, lấy từ TempData
            if (string.IsNullOrEmpty(diemDi) && TempData["DiemDi"] != null)
            {
                diemDi = TempData["DiemDi"].ToString();
                TempData.Keep("DiemDi");
            }
            if (string.IsNullOrEmpty(diemDen) && TempData["DiemDen"] != null)
            {
                diemDen = TempData["DiemDen"].ToString();
                TempData.Keep("DiemDen");
            }

            if (string.IsNullOrEmpty(diemDi) || string.IsNullOrEmpty(diemDen))
            {
                TempData["Error"] = "Vui lòng chọn tuyến đường";
                return RedirectToAction("SelectRoute");
            }

            TempData["DiemDi"] = diemDi;
            TempData["DiemDen"] = diemDen;
            TempData.Keep("DiemDi");
            TempData.Keep("DiemDen");

            // 👉 LẤY DANH SÁCH NGÀY GIỜ CÓ SẴN CHO TUYẾN ĐƯỢC CHỌN
            var availableDateTimes = _repository.GetAll()
                .Where(t => t.DiemDi == diemDi && t.DiemDen == diemDen)
                .Select(t => t.NgayDi)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            ViewBag.DiemDi = diemDi;
            ViewBag.DiemDen = diemDen;
            ViewBag.AvailableDateTimes = availableDateTimes;
            return View("SelectDateTime");
        }

        // 🚌 BƯỚC 3: Chọn hạng xe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectBusClass(string diemDi, string diemDen, DateTime ngayGio, string selectedSeats = "", string submitType = "selectBusClass")
        {
            if (string.IsNullOrEmpty(diemDi) || string.IsNullOrEmpty(diemDen) || ngayGio == default)
            {
                TempData["Error"] = "Vui lòng chọn tuyến đường và ngày giờ";
                return RedirectToAction("SelectRoute");
            }

            // 👉 NẾU CHECKOUT ĐƯỢC CHỌN - KIỂM TRA ĐĂNG NHẬP VÀ REDIRECT
            if (submitType == "checkout")
            {
                var userId = HttpContext.Session.GetInt32("userId");
                if (!userId.HasValue)
                {
                    TempData["CheckoutData"] = System.Text.Json.JsonSerializer.Serialize(new 
                    { 
                        diemDi, 
                        diemDen, 
                        ngayGio, 
                        selectedSeats 
                    });
                    TempData["RedirectAfterLogin"] = "/Checkout/Index";
                    return RedirectToAction("Login", "Account");
                }

                // ✅ Lưu thông tin vào TempData để sử dụng sau
                TempData["DirectCheckoutDiemDi"] = diemDi;
                TempData["DirectCheckoutDiemDen"] = diemDen;
                TempData["DirectCheckoutNgayGio"] = ngayGio;
                TempData["DirectCheckoutSelectedSeats"] = selectedSeats;
                TempData.Keep("DirectCheckoutDiemDi");
                TempData.Keep("DirectCheckoutDiemDen");
                TempData.Keep("DirectCheckoutNgayGio");
                TempData.Keep("DirectCheckoutSelectedSeats");

                // 👉 REDIRECT ĐẾN ACTION ĐỂ XỬ LÝ CHECKOUT
                return RedirectToAction("ProcessDirectCheckout");
            }

            // 👉 FLOW BÌNH THƯỜNG: CHỌN HẠNG XE
            TempData["DiemDi"] = diemDi;
            TempData["DiemDen"] = diemDen;
            TempData["NgayGio"] = ngayGio;
            TempData.Keep("DiemDi");
            TempData.Keep("DiemDen");
            TempData.Keep("NgayGio");

            ViewBag.DiemDi = diemDi;
            ViewBag.DiemDen = diemDen;
            ViewBag.NgayGio = ngayGio;

            return View("SelectBusClass");
        }

        // 👉 BƯỚC 4: Xác nhận hạng xe và đi đến chọn ghế
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmBusClass(int busClassValue, string diemDi, string diemDen, DateTime ngayGio)
        {
            if (string.IsNullOrEmpty(diemDi) || string.IsNullOrEmpty(diemDen) || ngayGio == default)
            {
                TempData["Error"] = "Vui lòng chọn tuyến đường và ngày giờ";
                return RedirectToAction("SelectRoute");
            }

            TempData["SelectedBusClass"] = busClassValue;
            TempData["DiemDi"] = diemDi;
            TempData["DiemDen"] = diemDen;
            TempData["NgayGio"] = ngayGio;
            TempData.Keep("SelectedBusClass");
            TempData.Keep("DiemDi");
            TempData.Keep("DiemDen");
            TempData.Keep("NgayGio");
            return RedirectToAction("Create");
        }

        // 👉 Tạo vé mới (BƯỚC 4: Chọn ghế)
        public IActionResult Create()
        {
            int busClassValue = 0;
            string diemDi = "";
            string diemDen = "";
            DateTime ngayGio = default;

            // Lấy từ TempData (từ ConfirmBusClass flow)
            if (TempData["SelectedBusClass"] != null)
            {
                busClassValue = (int)TempData["SelectedBusClass"];
                TempData.Keep("SelectedBusClass");
            }

            if (TempData["DiemDi"] != null)
            {
                diemDi = TempData["DiemDi"].ToString();
                TempData.Keep("DiemDi");
            }

            if (TempData["DiemDen"] != null)
            {
                diemDen = TempData["DiemDen"].ToString();
                TempData.Keep("DiemDen");
            }

            if (TempData["NgayGio"] != null)
            {
                ngayGio = (DateTime)TempData["NgayGio"];
                TempData.Keep("NgayGio");
            }

            // Nếu không có TempData, quay lại SelectDateTime
            if (string.IsNullOrEmpty(diemDi) || string.IsNullOrEmpty(diemDen) || ngayGio == default)
            {
                TempData["Error"] = "Vui lòng chọn ngày giờ, tuyến đường và hạng xe";
                return RedirectToAction("SelectDateTime");
            }

            var selectedClass = (BusClass)busClassValue;
            var busInfo = new BusInfo(selectedClass);

            // Mark already booked seats for this route, date and bus class
            var bookedSeats = _repository.GetAll()
                .Where(t => t.HangXe == selectedClass 
                    && t.DiemDi == diemDi 
                    && t.DiemDen == diemDen
                    && t.NgayDi.Date == ngayGio.Date)
                .Select(t => t.SoGhe)
                .Distinct()
                .ToList();

            foreach (var seatCode in bookedSeats)
            {
                busInfo.MarkSeatAsBooked(seatCode);
            }

            ViewBag.SelectedBusClass = selectedClass;
            ViewBag.BusClassValue = busClassValue;
            ViewBag.DiemDi = diemDi;
            ViewBag.DiemDen = diemDen;
            ViewBag.NgayGio = ngayGio;
            return View(busInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ticket ticket, int busClassValue = 0, string submitType = "addToCart")
        {
            // Lấy từ TempData (flow mới: SelectDateTime → SelectRoute → ConfirmBusClass → Create)
            int tempBusClassValue = 0;
            string diemDi = "";
            string diemDen = "";
            DateTime ngayGio = default;

            if (TempData["SelectedBusClass"] != null)
            {
                tempBusClassValue = (int)TempData["SelectedBusClass"];
                busClassValue = tempBusClassValue;
            }

            if (TempData["DiemDi"] != null)
            {
                diemDi = TempData["DiemDi"].ToString();
                ticket.DiemDi = diemDi;
            }

            if (TempData["DiemDen"] != null)
            {
                diemDen = TempData["DiemDen"].ToString();
                ticket.DiemDen = diemDen;
            }

            if (TempData["NgayGio"] != null)
            {
                ngayGio = (DateTime)TempData["NgayGio"];
                ticket.NgayDi = ngayGio;
            }

            var selectedClass = (BusClass)busClassValue;
            ticket.HangXe = selectedClass;

            // Validate seat selection
            if (string.IsNullOrEmpty(ticket.SoGhe))
            {
                ModelState.AddModelError("SoGhe", "Vui lòng chọn ít nhất một ghế ngồi");
            }
            else
            {
                // Validate that selected seats are not duplicated
                var seats = ticket.SoGhe.Split(',').Select(s => s.Trim()).Distinct().ToList();
                if (seats.Count != ticket.SoGhe.Split(',').Length)
                {
                    ModelState.AddModelError("SoGhe", "Có ghế được chọn trùng lặp");
                }

                // Validate that selected seats are not already booked for this date
                var bookedSeats = _repository.GetAll()
                    .Where(t => t.NgayDi.Date == ticket.NgayDi.Date 
                        && t.HangXe == selectedClass
                        && t.DiemDi == ticket.DiemDi
                        && t.DiemDen == ticket.DiemDen)
                    .SelectMany(t => t.SoGhe.Split(',').Select(s => s.Trim()))
                    .Distinct()
                    .ToList();

                foreach (var seat in seats)
                {
                    if (bookedSeats.Contains(seat))
                    {
                        ModelState.AddModelError("SoGhe", $"Ghế {seat} đã được bán cho tuyến {ticket.DiemDi} → {ticket.DiemDen} vào ngày {ticket.NgayDi:dd/MM/yyyy}");
                        break;
                    }
                }

                // Update SoGhe to comma-separated values
                ticket.SoGhe = string.Join(",", seats);
            }

            // Validate required fields
            if (string.IsNullOrEmpty(ticket.TenKhachHang))
            {
                ModelState.AddModelError("TenKhachHang", "Tên khách hàng là bắt buộc");
            }

            if (string.IsNullOrEmpty(ticket.SoDienThoai))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại là bắt buộc");
            }

            if (string.IsNullOrEmpty(ticket.DiemDi))
            {
                ModelState.AddModelError("DiemDi", "Điểm đi là bắt buộc");
            }

            if (string.IsNullOrEmpty(ticket.DiemDen))
            {
                ModelState.AddModelError("DiemDen", "Điểm đến là bắt buộc");
            }

            if (ticket.NgayDi == default)
            {
                ModelState.AddModelError("NgayDi", "Ngày đi là bắt buộc");
            }

            // 👉 TÍNH GIÁ VÉ DỰA TRÊN SỐ GHẾ VÀ LOẠI XE (lấy giá thực từ DB)
            int numberOfSeats = 0;
            if (!string.IsNullOrEmpty(ticket.SoGhe))
            {
                numberOfSeats = ticket.SoGhe.Split(',').Length;
            }

            // Lấy giá vé thực từ database theo tuyến + hạng xe (không dùng giá cố định)
            var matchingTicket = _repository.GetAll()
                .FirstOrDefault(t => t.DiemDi == ticket.DiemDi 
                    && t.DiemDen == ticket.DiemDen 
                    && t.HangXe == selectedClass);
            decimal pricePerSeat = matchingTicket != null ? matchingTicket.GiaVe : TicketRepository.GetTicketPrice(selectedClass);
            ticket.GiaVe = pricePerSeat * numberOfSeats;

            if (ticket.GiaVe <= 0)
            {
                ModelState.AddModelError("GiaVe", "Giá vé phải lớn hơn 0");
            }

            // If validation fails, return to form with bus info and errors
            if (!ModelState.IsValid)
            {
                var busInfo = new BusInfo(selectedClass);
                var bookedSeats = _repository.GetAll()
                    .Where(t => t.HangXe == selectedClass
                        && t.DiemDi == ticket.DiemDi
                        && t.DiemDen == ticket.DiemDen
                        && t.NgayDi.Date == ticket.NgayDi.Date)
                    .Select(t => t.SoGhe)
                    .Distinct()
                    .ToList();
                foreach (var seatCode in bookedSeats)
                {
                    busInfo.MarkSeatAsBooked(seatCode);
                }

                // Pass errors to view
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                ViewBag.BusClass = selectedClass;
                ViewBag.BusClassValue = busClassValue;
                ViewBag.DiemDi = diemDi;
                ViewBag.DiemDen = diemDen;
                ViewBag.NgayGio = ngayGio;
                return View(busInfo);
            }

            // 👉 THÊM VÀO GIỎ HÀNG THAY VÌ DATABASE
            var route = $"{ticket.DiemDi} → {ticket.DiemDen}";
            var departureTimeStr = ticket.NgayDi.ToString("dd/MM/yyyy HH:mm");

            // ⚠️ KIỂM TRA: Ghế trùng trên cùng tuyến + cùng ngày
            var cart = _cartService.GetCart();
            var newSeats = ticket.SoGhe.Split(',').Select(s => s.Trim()).ToList();

            // Tìm tất cả item cùng tuyến + cùng ngày + cùng hạng xe
            var itemsOnSameRoute = cart.Where(x => 
                x.Route == route && 
                x.DepartureTime == departureTimeStr &&
                x.BusClass == ticket.HangXe.ToString()).ToList();

            // Kiểm tra xem có ghế trùng không
            var duplicateSeats = new List<string>();
            foreach (var item in itemsOnSameRoute)
            {
                var existingSeats = item.Seats.Split(',').Select(s => s.Trim()).ToList();
                var commonSeats = newSeats.Intersect(existingSeats).ToList();
                duplicateSeats.AddRange(commonSeats);
            }

            // 👉 NẾU CÓ GHẾ TRÙNG → HIỂN THỊ NHƯ ĐÃ BÁN, KHÔNG BÁOLỖI
            if (duplicateSeats.Count > 0)
            {
                var busInfo = new BusInfo(selectedClass);

                // Lấy ghế đã bán từ database
                var bookedSeats = _repository.GetAll()
                    .Where(t => t.HangXe == selectedClass
                        && t.DiemDi == ticket.DiemDi
                        && t.DiemDen == ticket.DiemDen
                        && t.NgayDi.Date == ticket.NgayDi.Date)
                    .Select(t => t.SoGhe)
                    .Distinct()
                    .ToList();

                // Đánh dấu ghế từ database
                foreach (var seatCode in bookedSeats)
                {
                    busInfo.MarkSeatAsBooked(seatCode);
                }

                // Cộng thêm ghế đã chọn trước (từ giỏ hàng)
                foreach (var duplicateSeat in duplicateSeats)
                {
                    busInfo.MarkSeatAsBooked(duplicateSeat);
                }

                ViewBag.SelectedSeatsFromCart = duplicateSeats;
                ViewBag.Message = $"📌 Bạn đã chọn ghế {string.Join(", ", duplicateSeats)} trên tuyến này. Chúng hiện được hiển thị như 'đã bán'. Hãy chọn ghế khác.";
                ViewBag.BusClass = selectedClass;
                ViewBag.BusClassValue = busClassValue;
                ViewBag.DiemDi = diemDi;
                ViewBag.DiemDen = diemDen;
                ViewBag.NgayGio = ngayGio;
                return View(busInfo);
            }

            // 👉 NẾU KHÔNG CÓ GHẾ TRÙNG → CHO PHÉP THÊM (cả các ghế khác trên cùng tuyến)

            // Tạo unique code dựa trên tất cả thông tin vé (bao gồm ghế)
            // Vì mỗi ghế khác nhau = item khác nhau
            var ticketCodeInput = $"{route}_{ticket.SoGhe}_{ticket.NgayDi:yyyyMMdd}_{ticket.HangXe}";
            var ticketCodeHash = GetHashCode(ticketCodeInput);
            var ticketCode = "TK" + ticketCodeHash;

            var cartItem = new CartItem
            {
                TicketCode = ticketCode,
                Route = route,
                Seats = ticket.SoGhe,
                Price = ticket.GiaVe,
                Quantity = numberOfSeats,  // 👈 SỐ LƯỢNG = SỐ GHẾ
                BusClass = ticket.HangXe.ToString(),
                DepartureTime = ticket.NgayDi.ToString("dd/MM/yyyy HH:mm")
            };

            _cartService.AddToCart(cartItem);

            // 👉 KIỂM TRA NBUTTON ĐƯỢC CLICK VÀ REDIRECT TƯƠNG ỨNG
            if (submitType == "checkout")
            {
                // ✅ Kiểm tra user đã đăng nhập chưa
                var userId = HttpContext.Session.GetInt32("userId");
                if (!userId.HasValue)
                {
                    // Nếu chưa đăng nhập, redirect đến login
                    TempData["SuccessMessage"] = $"✅ Thêm vé {route} vào giỏ hàng thành công!";
                    TempData["RedirectAfterLogin"] = "/Checkout/Index";
                    return RedirectToAction("Login", "Account");
                }

                // ✅ Người dùng đã đăng nhập → Sang thanh toán
                TempData["Success"] = $"✅ Thêm vé {route} vào giỏ hàng thành công!";
                return RedirectToAction("Index", "Checkout");
            }
            else
            {
                // 👉 DEFAULT: Thêm vào giỏ và xem giỏ
                TempData["Success"] = $"✅ Thêm vé {route} vào giỏ hàng thành công!";
                return RedirectToAction("Index", "Cart");
            }
        }

        // 👉 BƯỚC THANH TOÁN TRỰC TIẾP TỪ CHỌN GHẾ
        [Authorize("Admin", "User")]
        public IActionResult ProcessDirectCheckout()
        {
            // Lấy dữ liệu từ TempData
            var diemDi = TempData["DirectCheckoutDiemDi"]?.ToString();
            var diemDen = TempData["DirectCheckoutDiemDen"]?.ToString();
            var ngayGio = TempData["DirectCheckoutNgayGio"];
            var selectedSeatsStr = TempData["DirectCheckoutSelectedSeats"]?.ToString();

            // Kiểm tra dữ liệu
            if (string.IsNullOrEmpty(diemDi) || string.IsNullOrEmpty(diemDen) || string.IsNullOrEmpty(selectedSeatsStr))
            {
                TempData["Error"] = "❌ Dữ liệu không hợp lệ. Vui lòng chọn lại!";
                return RedirectToAction("SelectRoute");
            }

            var seats = selectedSeatsStr.Split(',').ToList();
            if (seats.Count == 0)
            {
                TempData["Error"] = "❌ Vui lòng chọn ít nhất một ghế!";
                return RedirectToAction("SelectRoute");
            }

            // ✅ THÊM VÀO GIỎ HÀNG VỚI THÔNG TIN TỪ SELECTROUTE
            var route = $"{diemDi} → {diemDen}";
            var departureTimeStr = ((DateTime)ngayGio).ToString("dd/MM/yyyy HH:mm");

            // 👉 Lấy giá vé thực từ database theo tuyến
            var matchingTicket = _repository.GetAll()
                .FirstOrDefault(t => t.DiemDi == diemDi && t.DiemDen == diemDen);
            decimal ticketPrice = matchingTicket != null ? matchingTicket.GiaVe : 300000;
            decimal totalPrice = ticketPrice * seats.Count;

            // Tạo ticket code
            var ticketCodeInput = $"{route}_{selectedSeatsStr}_{((DateTime)ngayGio):yyyyMMdd}_Standard";
            var ticketCodeHash = GetHashCode(ticketCodeInput);
            var ticketCode = "TK" + ticketCodeHash;

            var cartItem = new CartItem
            {
                TicketCode = ticketCode,
                Route = route,
                Seats = selectedSeatsStr,
                Price = totalPrice,
                Quantity = seats.Count,
                BusClass = "Standard",
                DepartureTime = departureTimeStr
            };

            _cartService.AddToCart(cartItem);

            TempData["Success"] = $"✅ Thêm vé {route} vào giỏ hàng thành công!";
            return RedirectToAction("Index", "Checkout");
        }

        [HttpGet]
        public IActionResult GetBookedSeats(DateTime date, int busClass, string diemDi, string diemDen)
        {
            // Get seats booked for this specific date, bus class, AND route
            var bookedSeats = _repository.GetAll()
                .Where(t => t.NgayDi.Date == date.Date 
                    && t.HangXe == (BusClass)busClass
                    && t.DiemDi == diemDi
                    && t.DiemDen == diemDen)
                .SelectMany(t => t.SoGhe.Split(',').Select(s => s.Trim()))
                .Distinct()
                .ToList();

            return Json(bookedSeats);
        }

        // 🔐 Helper: Tạo hash code từ string
        private string GetHashCode(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                // Lấy 8 bytes đầu và convert thành hex string
                return BitConverter.ToString(hashedBytes, 0, 8).Replace("-", "");
            }
        }

        // 🔍 API Endpoint: Lấy danh sách các thành phố có sẵn
        [HttpGet]
        public IActionResult GetAvailableCities(string search = "")
        {
            // Lấy tuyến đường từ database
            var dbRoutes = _repository.GetAll()
                .GroupBy(t => new { t.DiemDi, t.DiemDen })
                .Select(g => new { g.Key.DiemDi, g.Key.DiemDen })
                .Distinct()
                .ToList();

            // Tuyến mặc định
            var defaultRoutes = new List<dynamic>
            {
                new { DiemDi = "Hà Nội", DiemDen = "HCM" },
                new { DiemDi = "Hà Nội", DiemDen = "Đà Nẵng" },
                new { DiemDi = "Hà Nội", DiemDen = "Hải Phòng" },
                new { DiemDi = "HCM", DiemDen = "Đà Nẵng" },
                new { DiemDi = "HCM", DiemDen = "Cần Thơ" },
                new { DiemDi = "Đà Nẵng", DiemDen = "Quy Nhơn" }
            };

            // Merge routes
            var allRoutes = dbRoutes.Cast<dynamic>().ToList();
            foreach (var defaultRoute in defaultRoutes)
            {
                if (!dbRoutes.Any(r => r.DiemDi == defaultRoute.DiemDi && r.DiemDen == defaultRoute.DiemDen))
                {
                    allRoutes.Add(defaultRoute);
                }
            }

            // Lấy tất cả các thành phố duy nhất
            var allCities = new HashSet<string>();
            foreach (var route in allRoutes)
            {
                allCities.Add((string)route.DiemDi);
                allCities.Add((string)route.DiemDen);
            }

            // Lọc theo search term nếu có
            var filteredCities = allCities
                .Where(city => string.IsNullOrEmpty(search) || city.Contains(search, StringComparison.OrdinalIgnoreCase))
                .OrderBy(city => city)
                .ToList();

            return Json(filteredCities);
        }
    }
}