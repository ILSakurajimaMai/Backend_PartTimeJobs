# Tài Liệu Kỹ Thuật Backend - PartTimeJobs API (ASP.NET Core)

Chào mừng bạn đến với mô tả kỹ thuật chi tiết của hệ thống Backend trong dự án JobFind. Tài liệu này đặc biệt tập trung giải phẫu cách vận hành của Hệ thống định danh người dùng (Đăng ký/Đăng nhập), tính năng Phân quyền (RBAC), và luồng xử lý Hồ sơ (Profile).

---

## 1. Hệ Thống Định Danh & Xác Thực (Đăng ký / Đăng nhập)

Tiến trình tạo điểm chạm và bảo mật tài khoản được xử lý khép kín trong tầng **AuthService**.

### Quá trình Đăng Ký (Register)
Khi Mobile App nhấn "SIGN UP", JSON bắn vào `[HttpPost("register")]` trên `AuthController` chặn lại và đùn xuống `AuthService.RegisterAsync()`:
1. **Validation Mở Rộng**: Code bắt buộc kiểm tra Email đã tồn tại trong DB chưa.
2. **Ánh xạ Quyền (Role Check)**: Thuật toán nhận text Role từ Mobile (`STUDENT` hoặc `EMPLOYER`), sau đó truy tìm khóa ngoại ID của Role này trong bảng `Roles` hệ thống.
3. **Mã Hóa & Khởi Tạo**: 
   - Giải thuật **BCrypt** băm nát cấu trúc Password gốc thành một mã Hash siêu bảo mật một chiều để lưu xuống bảng `Users`.
   - Kết dính tài khoản vừa tạo với khóa RoleId tương ứng xuống bảng `UserRoles`.
4. **Cơ chế Phân Tách Auto-Trigger (Ánh xạ Mapping DB)**: 
   - Nếu Role là `STUDENT`: Hệ thống tự động đẻ ra một bản ghi `Profile` mới rỗng tuếch map 1-1 với `UserId`.
   - Nếu Role là `EMPLOYER`: Hệ thống tự đẻ ra một `Company` với `OwnerId` map với `UserId`.
   - Việc sinh đòn bẩy tự động này giúp không bao giờ có tài khoản rác bị dư/thiếu thông tin liên kết!

### Quá trình Đăng Nhập (Login)
1. Truy vấn `User` bằng Email. Vượt qua lớp rào chắn kiểm tra `IsActive` (tài khoản có bị ban hay không).
2. Code check giải ngược hash bằng hàm `BCrypt.Verify()`. 
3. Nếu hợp lệ, hệ thống sẽ thực hiện truy vấn nội suy gom tất cả các Role của User nén lại.
4. **Giao màng Token (JWT)**: `JwtService` sẽ mã hóa ID User và list Roles lên trên 1 chuỗi `AccessToken`, đồng thời cấp luôn 1 `RefreshToken` nhét vào DB cấu hình tồn tại hàng chục ngày. Gửi lại máy khách trọn vẹn cả cụm Token này.

---

## 2. Hệ Thống Phân Quyền Vai Trò (Roles & RBAC)

Hệ thống hoạt động trên nguyên tắc **Role-Based Access Control** qua JWT:
* Trái tim của Authorization nằm ngay trên Controller. Ví dụ: Khi khai báo C# attribute **`[Authorize(Roles = "STUDENT,ADMIN")]`** ngay trên đầu hàm trong Backend.
* Mỗi khi điện thoại gửi Request lên kèm Token. Tầng Pipeline Middleware của ASP.NET sẽ tự bóc tách chuỗi Token ra => Bóc lớp Claims chứa chữ "Role" ra => Đem đi so sánh với luật của hàm: 
  * _Anh là Employer? Role không đúng => Báo lỗi truy cập HTTP 403 Forbidden chặn đuổi ngay lập tức mà không thèm chạy code trong hàm._
  * _Anh là Student? Ok được cấp vé thông hành._

---

## 3. Hoạt động của Profile (Hồ Sơ User)

Module Profile là ví dụ điển hình nhất cho chuẩn Restful API và Design Patterns 3-Tier.

### Ánh xạ Entity - Database (EF Core)
* Trong Database, bảng `Profiles` chứa cục bộ dữ liệu cá nhân (FirstName, LastName, Address, Bio, DO...B). Nó là một bảng nhánh nối vào bảng `Users`. Nhưng để che giấu bớt những property nhạy cảm, **`ProfileDto`** (Data Transfer Object) chỉ hứng và chứa các field cần thiết để đưa cho Frontend xem, tối ưu hóa kích thước chuỗi JSON.

### Luồng Hoạt Động của Logic (ProfileService)
* **`CreateOrUpdateAsync()`**: Đây là một thiết kế thông minh (Upsert). Thay vì chia thành 2 API, Service tự tra ID của User. 
  - Đã có Profile -> Cập nhật (Update EF tracking).
  - Chưa có Profile (trường hợp đăng ký bị lag) -> Thêm mới tinh ghim lại mã User.

### Cơ Chế Bảo Vệ ở Controller (ProfilesController)
* **Hành Vi Tuyệt Mật**: 
  Thủ thuật chặn ăn cắp profile bằng hàm **`GetUserId()`**. Hàm này đào trực tiếp `UserId` lấy trong lõi Token (thuộc tính `ClaimTypes.NameIdentifier`). 
  Như vậy, Mobile gửi yêu cầu sửa đổi, Controller **hiểu là ai lập tức**, không cần gửi Id User bên ngoài body, hoàn toàn triệt tiêu được lỗi Hacker gửi Id User khác báo API sửa thành thông tin của mình.
* **Hoạt động Logger**: 
  Ngay trước Return thành công, một lớp `_activityLogService.LogActivityAsync` sẽ âm thầm thu lượm địa chỉ IP `RemoteIpAddress`, Trình duyệt (`User-Agent`) và phương thức `GET/POST` để ghi sổ Activity Log nhằm mục đích Admin theo dõi hoạt động người tham gia!
