# CLAUDE.md

Guidance for AI assistants working with this repository.

## Project Overview

**Part-Time Job Finding Platform** — REST API built with **.NET 10** using **Clean Architecture**.

```
PTJ.Domain        → Core entities, interfaces, business logic (no dependencies)
PTJ.Application   → Services interfaces, DTOs, AutoMapper (depends on Domain only)
PTJ.Infrastructure → EF Core, repositories, service implementations (depends on App + Domain)
PTJ.API           → Controllers, middleware, SignalR hubs, DI config (depends on all)
```

---

## Architecture Patterns

### Repository Pattern & Unit of Work

All data access goes through `IUnitOfWork`:

```csharp
var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == email);
await _unitOfWork.SaveChangesAsync();
```

- `IRepository<T>` — generic CRUD (`FindAsync`, `FirstOrDefaultAsync`, `AddAsync`, `Update`, `Remove`)
- `IUnitOfWork` — exposes typed repositories, manages transactions
- Implementation: `PTJ.Infrastructure/Repositories/GenericRepository.cs`, `UnitOfWork.cs`

### Base Entities

All entities inherit from `BaseAuditableEntity` → `BaseEntity`:

| Property | Type | Notes |
|---|---|---|
| `Id` | int | PK, auto-increment |
| `CreatedAt` | DateTime | UTC, auto-set |
| `UpdatedAt` | DateTime | UTC, auto-set |
| `IsDeleted` | bool | Soft delete flag |
| `RowVersion` | byte[] | Optimistic concurrency |

`AppDbContext.SaveChangesAsync()` sets timestamps automatically. Soft deletes are enforced — entities are never physically removed unless explicitly stated in migrations.

### Result Pattern

All service methods return `Result<T>`:

```csharp
public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; }
}
```

Controllers map `Result` to HTTP responses:
- `result.Success = false` → `BadRequest` or `NotFound`
- `result.Success = true` → `Ok` or `CreatedAtAction`

### Authentication

- **JWT Bearer**: Access token (60 min) + Refresh token (7 days)
- Config: `appsettings.json` → `"Jwt"` section
- Services: `IJwtService` (token generation), `IAuthService` (login/register/refresh)
- Role-based: `[Authorize(Roles = "STUDENT,ADMIN")]`
- Custom filter: `AuthorizeCompanyOwnerFilter` (company ownership check)

---

## Domain Entities

### Auth (`auth` schema)

| Entity | Description |
|---|---|
| `User` | Base user (Email, PasswordHash, FullName, IsLocked) |
| `Role` | ADMIN (id=1), EMPLOYER (id=2), STUDENT (id=3) |
| `UserRole` | Many-to-many join |
| `RefreshToken` | JWT refresh tokens |

### Seeker (`seeker` schema)

| Entity | Description |
|---|---|
| `Profile` | Basic personal info — **1-1 with User**. Created automatically on STUDENT registration. Fields: `FullName`, `DateOfBirth`, `PhoneNumber`, `Email`, `Address`. |
| `CV` | Detailed professional profile — **1-N with User and Profile**. Each user can have multiple CVs; one is marked `IsDefault`. |
| `CVSkill` | Skills in a CV |
| `CVExperience` | Work experience in a CV |
| `CVEducation` | Education history in a CV |
| `CVCertificate` | Certificates in a CV |

### Jobs (`jobs` schema)

| Entity | Description |
|---|---|
| `JobPost` | Job posting with title, description, salary, location, status |
| `JobShift` | Work shifts (start/end time, day of week) |
| `JobPostSkill` | Required skills for a job |
| `Application` | Job application (STUDENT → JobPost), references a CV |
| `ApplicationHistory` | Status change history |
| `ApplicationStatusLookup` | Reference: Pending/Reviewing/Accepted/Rejected/Withdrawn |

### Companies (`companies` schema)

| Entity | Description |
|---|---|
| `Company` | Company profile (created only after admin approval) |
| `CompanyRegistrationRequest` | Pending company registration request |

### Chat (`chat` schema)

| Entity | Description |
|---|---|
| `ChatConversation` | Conversation between 2 users (with optional JobPost context) |
| `ChatMessage` | Individual messages |

### Logging (`logging` schema)

| Entity | Description |
|---|---|
| `UserActivityLog` | HTTP request log (userId, method, path, statusCode, durationMs) |
| `SystemErrorLog` | Unhandled exceptions (level, message, stackTrace) |

---

## Service Layer

Each feature has interface in `PTJ.Application/Services/` and implementation in `PTJ.Infrastructure/Services/`:

| Interface | Implementation | Responsibility |
|---|---|---|
| `IAuthService` | `AuthService` | Login, register, refresh tokens |
| `IProfileService` | `ProfileService` | Basic profile (1-1 User) — get/update |
| `ICVService` | `CVService` | CV management — CRUD, set-default |
| `IJobPostService` | `JobPostService` | Job posting CRUD, search |
| `ICompanyService` | `CompanyService` | Company management, registration requests |
| `IApplicationService` | `ApplicationService` | Job application workflow |
| `IChatService` | `ChatService` | Chat conversations and messages |
| `IFileStorageService` | `LocalFileStorageService` | File upload/download |
| `ISearchService` | `SearchService` | Cross-entity search |
| `IActivityLogService` | `ActivityLogService` | Activity log write/read |
| `IErrorLogService` | `ErrorLogService` | Error log write/read |

---

## User Registration Workflow

**CRITICAL**: Follow this when working with registration or company creation.

### Student Registration (`POST /api/auth/register` with role=STUDENT):
1. Create `User` entity
2. Assign **STUDENT** role
3. Create **`Profile`** entity (FullName, Email, PhoneNumber from registration DTO)
4. Create default **`CV`** entity linked to Profile (`ProfileId`) and User (`UserId`), `IsDefault = true`

### Company Registration Request (`POST /api/companies`):
- Any authenticated user can submit
- Creates `CompanyRegistrationRequest` (status = Pending)
- **No `Company` entity created yet**
- **No EMPLOYER role assigned yet**

### Admin Approval (`POST /api/companyrequests/approve`):
- Creates `Company` entity linked to user
- Sets request status = Approved
- Assigns **EMPLOYER** role to user
- User now has both STUDENT + EMPLOYER roles

---

## AutoMapper

Mapping profiles in `PTJ.Application/Mapping/MappingCV.cs` (class `MappingCV : AutoMapper.Profile`).

**Important**: Use `PTJ.Domain.Entities.Profile` (fully qualified) to avoid ambiguity with `AutoMapper.Profile`.

Registered in DI: `builder.Services.AddAutoMapper(typeof(MappingCV))`

---

## API Controllers

All in `PTJ.API/Controllers/`:

| Controller | Route | Roles |
|---|---|---|
| `AuthController` | `/api/auth` | Public |
| `ProfileController` | `/api/profile` | Authenticated |
| `CVsController` | `/api/cvs` | Public (read), STUDENT/ADMIN (write) |
| `JobPostsController` | `/api/jobposts` | Public (read), EMPLOYER/ADMIN (write) |
| `CompaniesController` | `/api/companies` | Public (read), EMPLOYER/ADMIN (write) |
| `CompanyRequestsController` | `/api/companyrequests` | ADMIN |
| `ApplicationsController` | `/api/applications` | Authenticated |
| `ChatController` | `/api/chat` | Authenticated |
| `AIChatController` | `/api/aichat` | Authenticated |
| `FilesController` | `/api/files` | Public (download), Authenticated (upload/delete) |
| `AdminController` | `/api/admin` | ADMIN |
| `LogsController` | `/api/logs` | ADMIN |

---

## Real-time Chat (SignalR)

**Hub:** `PTJ.API/Hubs/ChatHub.cs`
- WebSocket endpoint: `ws://localhost:5000/hubs/chat?access_token=JWT`
- JWT auth via query string
- User auto-joined to group by userId

---

## Middleware & Filters

| Component | Purpose |
|---|---|
| `GlobalExceptionMiddleware` | Catch unhandled exceptions → standardized JSON + log to DB |
| `RequestLoggingMiddleware` | Log every HTTP request to `UserActivityLog` |
| `ValidationFilter` | Global model validation filter |
| `AuthorizeCompanyOwnerFilter` | Ensure EMPLOYER can only modify their own company data |

---

## File Storage

- Local storage, configured in `appsettings.json` → `"FileStorage"`
- Default upload path: `Uploads/`
- Max size: 10MB
- Allowed: `.jpg`, `.jpeg`, `.png`, `.pdf`, `.doc`, `.docx`
- Metadata tracked in `FileEntity` table

---

## Database

- **SQL Server** via **EF Core 10**
- Connection string: `appsettings.json` → `"ConnectionStrings:Default"`
- Fluent API configs: `PTJ.Infrastructure/Configurations/`
- `AppDbContext` loads all via `ApplyConfigurationsFromAssembly`

### Key EF Core Quirks

- **Soft delete query filters** registered on all entities: `HasQueryFilter(e => !e.IsDeleted)`
- **RowVersion / concurrency**: If SQL triggers interfere with EF's `OUTPUT` clause, use `.UseUpdateRowVersion()` or drop conflicting triggers
- **Schema separation**: Each domain uses its own schema (`auth`, `seeker`, `jobs`, `companies`, `chat`, `logging`)

---

## Common Commands

```bash
# Build
dotnet build

# Run
dotnet run --project PTJ.API

# Add migration
dotnet ef migrations add <Name> --project PTJ.Infrastructure --startup-project PTJ.API

# Apply migration
dotnet ef database update --project PTJ.Infrastructure --startup-project PTJ.API

# Remove last migration
dotnet ef migrations remove --project PTJ.Infrastructure --startup-project PTJ.API
```

---

## Important Notes

- All timestamps: **UTC** (`DateTime.UtcNow`)
- Password hashing: **BCrypt** (`BCrypt.Net-Next`)
- Concurrency: **RowVersion** (optimistic concurrency, byte[])
- Soft deletes enforced everywhere — check `IsDeleted` in raw queries
- CORS: Configured for SignalR — must use `AllowCredentials()` + explicit origins (no `AllowAnyOrigin`)
- Swagger: Available at `/swagger` in development, JWT Bearer auth pre-configured
- **Never log passwords or tokens**
- Search uses `EF.Functions.Like()` — executes at DB level, not in-memory
