# Tài liệu API - Job Students Platform

**Base URL:** `/api`

---

## 1. Authentication (Xác thực)

### 1.1 Đăng ký tài khoản

**`POST /api/auth/register`** — Public

Đăng ký tài khoản mới (STUDENT hoặc EMPLOYER). Khi STUDENT đăng ký, hệ thống tự động tạo **Profile** (thông tin cá nhân) và **CV mặc định**.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "fullName": "Nguyen Van A",
  "phoneNumber": "0912345678",
  "role": "STUDENT"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "userId": 1,
    "email": "user@example.com",
    "fullName": "Nguyen Van A",
    "role": "STUDENT"
  },
  "message": "Registration successful. Please login to continue."
}
```

---

### 1.2 Đăng nhập

**`POST /api/auth/login`** — Public

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "userId": 1,
    "email": "user@example.com",
    "fullName": "Nguyen Van A",
    "roles": ["STUDENT"],
    "accessToken": "eyJhbGci...",
    "refreshToken": "abc123xyz...",
    "expiresAt": "2026-10-27T10:00:00Z"
  }
}
```

---

### 1.3 Refresh Token

**`POST /api/auth/refresh`** — Public

**Request Body:**
```json
{
  "refreshToken": "abc123xyz..."
}
```

---

### 1.4 Đăng xuất

**`POST /api/auth/revoke`** — Authenticated

**Request Body:**
```json
{
  "refreshToken": "abc123xyz..."
}
```

---

## 2. Profile (Thông tin cá nhân)

Profile là thông tin cơ bản của user (1-1 với User). Được tạo tự động khi STUDENT đăng ký. Không thể tạo thêm hay xóa — chỉ xem và cập nhật.

### 2.1 Lấy profile của tôi

**`GET /api/profile/me`** — Authenticated

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "userId": 5,
    "fullName": "Nguyen Van A",
    "dateOfBirth": "2000-06-15T00:00:00",
    "phoneNumber": "0912345678",
    "email": "user@example.com",
    "address": "123 Nguyen Hue, Q1, HCMC"
  }
}
```

---

### 2.2 Cập nhật profile

**`PUT /api/profile/me`** — Authenticated

**Request Body:**
```json
{
  "fullName": "Nguyen Van A",
  "dateOfBirth": "2000-06-15T00:00:00",
  "phoneNumber": "0912345678",
  "email": "user@example.com",
  "address": "123 Nguyen Hue, Q1, HCMC"
}
```

---

## 3. CVs (Hồ sơ ứng viên chi tiết)

CV là hồ sơ chuyên môn đầy đủ (1-N với User và Profile). Mỗi user có thể có nhiều CV, trong đó có 1 CV mặc định được dùng khi nộp đơn.

### 3.1 Lấy CV theo ID

**`GET /api/cvs/{id}`** — Public

---

### 3.2 Lấy CV mặc định của tôi

**`GET /api/cvs/me`** — `STUDENT`, `ADMIN`

Trả về CV đang được đặt làm mặc định.

---

### 3.3 Lấy tất cả CV của tôi

**`GET /api/cvs/my`** — `STUDENT`, `ADMIN`

Trả về danh sách tất cả CV của user hiện tại (sắp xếp: CV mặc định lên đầu).

---

### 3.4 Tạo CV mới

**`POST /api/cvs`** — `STUDENT`, `ADMIN`

**Request Body:**
```json
{
  "title": "CV Lập trình viên Backend",
  "targetPosition": "Backend Developer",
  "isDefault": false,
  "fullName": "Nguyen Van A",
  "email": "user@example.com",
  "dateOfBirth": "2000-01-01T00:00:00",
  "gender": 0,
  "address": "123 Nguyen Hue, Q1, HCMC",
  "phoneNumber": "0912345678",
  "studentId": "SE12345",
  "university": "FPT University",
  "major": "Software Engineering",
  "gpa": 3.5,
  "yearOfStudy": 3,
  "expectedGraduationDate": "2027-06-01T00:00:00",
  "resumeUrl": "https://example.com/cv.pdf",
  "bio": "Sinh viên năm 3 yêu thích lập trình...",
  "linkedInUrl": "https://linkedin.com/in/nguyenvana",
  "gitHubUrl": "https://github.com/nguyenvana",
  "skills": [
    {
      "skillName": "C#",
      "proficiencyLevel": 4,
      "yearsOfExperience": 2
    },
    {
      "skillName": "ASP.NET Core",
      "proficiencyLevel": 3,
      "yearsOfExperience": 1
    }
  ],
  "experiences": [
    {
      "companyName": "ABC Tech",
      "position": "Backend Intern",
      "description": "Phát triển API với .NET Core",
      "startDate": "2024-01-01T00:00:00",
      "endDate": "2024-06-30T00:00:00",
      "isCurrentlyWorking": false
    }
  ],
  "educations": [
    {
      "institutionName": "FPT University",
      "degree": "Bachelor",
      "fieldOfStudy": "Software Engineering",
      "startDate": "2021-09-01T00:00:00",
      "endDate": "2025-06-30T00:00:00",
      "gpa": 3.5,
      "description": "Chuyên ngành AI"
    }
  ],
  "certificates": [
    {
      "name": "AWS Certified Developer",
      "issuingOrganization": "Amazon",
      "issueDate": "2024-01-01T00:00:00",
      "expiryDate": "2027-01-01T00:00:00",
      "credentialId": "ABC123",
      "credentialUrl": "https://aws.amazon.com/verify",
      "certificateFileUrl": "https://example.com/cert.pdf"
    }
  ]
}
```

**Lưu ý:** Nếu `title` bỏ trống → tự động đặt `"CV 1"`, `"CV 2"`, ... Nếu là CV đầu tiên → tự động đặt `isDefault = true`.

---

### 3.5 Cập nhật CV

**`PUT /api/cvs/{id}`** — `STUDENT`, `ADMIN`

**Request Body:** Tương tự Tạo CV.

---

### 3.6 Đặt CV mặc định

**`POST /api/cvs/{id}/set-default`** — `STUDENT`, `ADMIN`

Đặt CV này làm CV mặc định (các CV khác sẽ bị bỏ mặc định).

---

### 3.7 Xóa CV

**`DELETE /api/cvs/{id}`** — `STUDENT`, `ADMIN`

Nếu xóa CV mặc định → CV còn lại đầu tiên sẽ được tự động đặt làm mặc định.

---

## 4. Job Posts (Tin tuyển dụng)

### 4.1 Lấy danh sách tin tuyển dụng

**`GET /api/jobposts`** — Public

| Query Param | Mô tả | Mặc định |
|---|---|---|
| `pageNumber` | Trang | 1 |
| `pageSize` | Số lượng mỗi trang | 10 |

---

### 4.2 Lấy chi tiết tin tuyển dụng

**`GET /api/jobposts/{id}`** — Public

---

### 4.3 Tìm kiếm tin tuyển dụng

**`GET /api/jobposts/search`** — Public

| Query Param | Mô tả | Mặc định |
|---|---|---|
| `searchTerm` | Từ khóa (title, description, location) | — |
| `pageNumber` | | 1 |
| `pageSize` | | 10 |
| `sortDescending` | Sắp xếp mới nhất trước | true |

---

### 4.4 Lấy tin tuyển dụng theo công ty

**`GET /api/jobposts/company/{companyId}`** — Public

---

### 4.5 Tạo tin tuyển dụng

**`POST /api/jobposts`** — `EMPLOYER`, `ADMIN`

**Request Body:**
```json
{
  "title": "Part-time English Tutor",
  "description": "Dạy tiếng Anh cho trẻ em...",
  "requirements": "IELTS 6.5+",
  "benefits": "Lịch linh hoạt",
  "salaryMin": 50000,
  "salaryMax": 100000,
  "salaryPeriod": "Hour",
  "location": "Ho Chi Minh City",
  "workType": "Part-time",
  "category": "Education",
  "numberOfPositions": 5,
  "applicationDeadline": "2026-12-31T23:59:59",
  "shifts": [
    { "startTime": "18:00", "endTime": "20:00", "dayOfWeek": "Monday" }
  ],
  "requiredSkills": ["English", "Teaching"]
}
```

---

### 4.6 Cập nhật tin tuyển dụng

**`PUT /api/jobposts/{id}`** — `EMPLOYER`, `ADMIN`

---

### 4.7 Xóa tin tuyển dụng

**`DELETE /api/jobposts/{id}`** — `EMPLOYER`, `ADMIN`

---

### 4.8 Thay đổi trạng thái

**`PATCH /api/jobposts/{id}/status`** — `EMPLOYER`, `ADMIN`

**Request Body:**
```json
1
```
`0` = Draft, `1` = Open, `2` = Closed

---

## 5. Applications (Đơn ứng tuyển)

### 5.1 Lấy chi tiết đơn ứng tuyển

**`GET /api/applications/{id}`** — Authenticated

---

### 5.2 Lấy đơn ứng tuyển theo Job Post

**`GET /api/applications/job/{jobPostId}`** — `EMPLOYER`, `ADMIN`

| Query Param | Mặc định |
|---|---|
| `pageNumber` | 1 |
| `pageSize` | 10 |

---

### 5.3 Lấy danh sách đơn của tôi

**`GET /api/applications/me`** — `STUDENT`, `ADMIN`

| Query Param | Mô tả | Mặc định |
|---|---|---|
| `pageNumber` | | 1 |
| `pageSize` | | 10 |
| `cvId` | Lọc theo CV cụ thể | — |

---

### 5.4 Nộp đơn ứng tuyển

**`POST /api/applications`** — `STUDENT`, `ADMIN`

**Request Body:**
```json
{
  "jobPostId": 123,
  "profileId": 5,
  "coverLetter": "Tôi rất quan tâm đến vị trí này...",
  "resumeUrl": "https://example.com/cv.pdf"
}
```

`profileId` (optional) — ID của CV muốn dùng để ứng tuyển. Nếu bỏ trống → dùng CV mặc định.

---

### 5.5 Cập nhật trạng thái đơn (Employer)

**`PATCH /api/applications/{id}/status`** — `EMPLOYER`, `ADMIN`

**Request Body:**
```json
{
  "statusId": 3,
  "notes": "Ứng viên phù hợp"
}
```

| statusId | Trạng thái |
|---|---|
| 1 | Pending (Chờ duyệt) |
| 2 | Reviewing (Đang xem xét) |
| 3 | Accepted (Chấp nhận) |
| 4 | Rejected (Từ chối) |
| 5 | Withdrawn (Đã rút) |

---

### 5.6 Rút đơn ứng tuyển

**`POST /api/applications/{id}/withdraw`** — `STUDENT`, `ADMIN`

---

## 6. Companies (Công ty)

### 6.1 Lấy danh sách công ty

**`GET /api/companies`** — Public

| Query Param | Mặc định |
|---|---|
| `pageNumber` | 1 |
| `pageSize` | 10 |

---

### 6.2 Lấy chi tiết công ty

**`GET /api/companies/{id}`** — Public

---

### 6.3 Tìm kiếm công ty

**`GET /api/companies/search`** — Public

| Query Param | Mô tả |
|---|---|
| `searchTerm` | Từ khóa (name, description, industry, address) |
| `pageNumber` | |
| `pageSize` | |
| `sortDescending` | |

---

### 6.4 Lấy công ty của tôi

**`GET /api/companies/me`** — `EMPLOYER`, `ADMIN`

---

### 6.5 Đăng ký công ty

**`POST /api/companies`** — Authenticated

Gửi yêu cầu đăng ký (cần Admin duyệt, chưa tạo Company ngay).

**Request Body:**
```json
{
  "name": "Tech Solutions Ltd.",
  "description": "Software Outsourcing",
  "address": "District 1, HCMC",
  "website": "https://techsolutions.com",
  "taxCode": "0123456789",
  "industry": "IT",
  "employeeCount": 50,
  "foundedYear": "2015-01-01T00:00:00"
}
```

---

### 6.6 Cập nhật thông tin công ty

**`PUT /api/companies/{id}`** — `EMPLOYER`, `ADMIN`

---

### 6.7 Xóa công ty

**`DELETE /api/companies/{id}`** — `EMPLOYER`, `ADMIN`

---

## 7. Company Requests (Duyệt công ty — Admin)

### 7.1 Danh sách yêu cầu chờ duyệt

**`GET /api/companyrequests/pending`** — `ADMIN`

| Query Param | Mặc định |
|---|---|
| `pageNumber` | 1 |
| `pageSize` | 10 |

---

### 7.2 Chi tiết yêu cầu

**`GET /api/companyrequests/{id}`** — `ADMIN`

---

### 7.3 Duyệt yêu cầu

**`POST /api/companyrequests/approve`** — `ADMIN`

**Request Body:**
```json
{ "requestId": 123 }
```

Khi duyệt: tạo Company, gán role EMPLOYER cho user.

---

### 7.4 Từ chối yêu cầu

**`POST /api/companyrequests/reject`** — `ADMIN`

**Request Body:**
```json
{
  "requestId": 123,
  "rejectionReason": "Thiếu tài liệu thuế"
}
```

---

## 8. Chat (Tin nhắn thời gian thực)

### 8.1 Tạo/Lấy conversation

**`POST /api/chat/conversations`** — Authenticated

**Request Body:**
```json
{
  "recipientId": 5,
  "jobPostId": 10
}
```

---

### 8.2 Danh sách conversations

**`GET /api/chat/conversations`** — Authenticated

| Query Param | Mặc định |
|---|---|
| `pageNumber` | 1 |
| `pageSize` | 20 |

---

### 8.3 Tin nhắn trong conversation

**`GET /api/chat/conversations/{conversationId}/messages`** — Authenticated

| Query Param | Mặc định |
|---|---|
| `pageNumber` | 1 |
| `pageSize` | 50 |

---

### 8.4 Gửi tin nhắn (REST)

**`POST /api/chat/messages`** — Authenticated

```json
{
  "conversationId": 10,
  "recipientId": 5,
  "jobPostId": 3,
  "content": "Xin chào, vị trí còn không?"
}
```

---

### 8.5 Đánh dấu đã đọc

**`POST /api/chat/conversations/{conversationId}/read`** — Authenticated

---

### 8.6 Số tin chưa đọc

**`GET /api/chat/unread-count`** — Authenticated

---

### 8.7 SignalR WebSocket

**Endpoint:** `ws://localhost:5000/hubs/chat?access_token=YOUR_JWT`

**Client → Server:**
```typescript
SendMessage(dto)              // Gửi tin nhắn
MarkAsRead(conversationId)    // Đánh dấu đã đọc
UpdateTyping(conversationId, isTyping)  // Trạng thái đang nhập
JoinConversation(conversationId)
LeaveConversation(conversationId)
```

**Server → Client:**
```typescript
ReceiveMessage(message)           // Tin nhắn mới
MessagesMarkedAsRead(conversationId)
UserTyping(userId, isTyping)
Error(message)
```

---

## 9. AI Chat (Trợ lý AI)

### 9.1 Gửi tin nhắn

**`POST /api/aichat/message`** — Authenticated

**Request Body:**
```json
{ "message": "Tìm việc part-time IT tại HCMC" }
```

**Response:**
```json
{
  "success": true,
  "data": "Dựa trên hồ sơ của bạn, tôi tìm thấy..."
}
```

---

### 9.2 Khởi động lại phiên chat

**`POST /api/aichat/restart`** — Authenticated

---

## 10. Files (Quản lý File)

### 10.1 Upload file

**`POST /api/files/upload`** — Authenticated  
**Content-Type:** `multipart/form-data`

| Form field | Giá trị |
|---|---|
| `file` | Binary file |
| `folder` | `avatars` \| `cvs` \| `logos` \| `certificates` \| `general` |

**Response:**
```json
{
  "success": true,
  "data": { "url": "/uploads/cvs/filename.pdf" }
}
```

Giới hạn: 10MB, định dạng: `.jpg`, `.jpeg`, `.png`, `.pdf`, `.doc`, `.docx`

---

### 10.2 Xóa file

**`DELETE /api/files?fileUrl=/uploads/cvs/filename.pdf`** — Authenticated

---

### 10.3 Download file

**`GET /api/files/download?fileUrl=/uploads/cvs/resume.pdf`** — Public

---

## 11. Admin (Quản trị)

### 11.1 Danh sách users

**`GET /api/admin/users`** — `ADMIN`

| Query Param | Mô tả |
|---|---|
| `search` | Từ khóa tìm kiếm |
| `pageNumber` | |
| `pageSize` | |

---

### 11.2 Khóa tài khoản

**`POST /api/admin/users/{id}/lock`** — `ADMIN`

---

### 11.3 Mở khóa tài khoản

**`POST /api/admin/users/{id}/unlock`** — `ADMIN`

---

### 11.4 Cập nhật trạng thái Job Post

**`PUT /api/admin/jobs/{id}/status`** — `ADMIN`

```json
{ "status": 1 }
```

---

### 11.5 Xóa Job Post

**`DELETE /api/admin/jobs/{id}`** — `ADMIN`

---

### 11.6 Thống kê Dashboard

**`GET /api/admin/stats`** — `ADMIN`

**Response:**
```json
{
  "totalUsers": 1000,
  "totalJobPosts": 250,
  "totalApplications": 5000,
  "activeJobs": 180
}
```

---

## 12. Logs (Nhật ký hệ thống — Admin)

### 12.1 Activity Logs

**`GET /api/logs/activities`** — `ADMIN`

| Query Param | Mô tả |
|---|---|
| `userId` | Lọc theo user |
| `startDate` | Từ ngày |
| `endDate` | Đến ngày |
| `pageNumber` | (max: 100) |
| `pageSize` | |

---

### 12.2 Error Logs

**`GET /api/logs/errors`** — `ADMIN`

| Query Param | Mô tả |
|---|---|
| `level` | `Critical` \| `Error` \| `Warning` |
| `startDate` | |
| `endDate` | |

---

### 12.3 Thống kê Activity

**`GET /api/logs/activities/stats`** — `ADMIN`

Query: `startDate`, `endDate`

---

### 12.4 Thống kê Error

**`GET /api/logs/errors/stats`** — `ADMIN`

---

## Phụ lục

### Response Format

```json
{
  "success": true | false,
  "message": "...",
  "data": { },
  "errors": []
}
```

---

### Enums

**Gender:** `0` = Male, `1` = Female, `2` = Other

**JobPostStatus:** `0` = Draft, `1` = Open, `2` = Closed

**ApplicationStatus:**

| ID | Trạng thái |
|---|---|
| 1 | Pending |
| 2 | Reviewing |
| 3 | Accepted |
| 4 | Rejected |
| 5 | Withdrawn |

**CompanyRequestStatus:** `0` = Pending, `1` = Approved, `2` = Rejected

**UserRole:** `STUDENT`, `EMPLOYER`, `ADMIN`

---

### Database Schema

| Schema | Bảng | Mô tả |
|---|---|---|
| `auth` | Users, Roles, UserRoles, RefreshTokens | Xác thực |
| `seeker` | Profiles | Profile cơ bản (1-1 User) |
| `seeker` | CVs, CVSkills, CVExperiences, CVEducations, CVCertificates | CV chi tiết (1-N User) |
| `jobs` | JobPosts, JobShifts, JobPostSkills | Tin tuyển dụng |
| `jobs` | Applications, ApplicationHistories, ApplicationStatuses | Đơn ứng tuyển |
| `companies` | Companies, CompanyRegistrationRequests | Công ty |
| `chat` | ChatConversations, ChatMessages | Tin nhắn |
| `ai` | AIChatSessions, AIChatMessages | AI Chat |
| `logging` | UserActivityLogs, SystemErrorLogs | Nhật ký |
