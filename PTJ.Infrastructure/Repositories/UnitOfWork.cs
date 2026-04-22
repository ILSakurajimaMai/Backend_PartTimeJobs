using Microsoft.EntityFrameworkCore.Storage;
using PTJ.Domain.Entities;
using PTJ.Domain.Interfaces;
using PTJ.Infrastructure.Persistence;

namespace PTJ.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    // Auth
    private IRepository<User>? _users;
    private IRepository<Role>? _roles;
    private IRepository<UserRole>? _userRoles;
    private IRepository<RefreshToken>? _refreshTokens;

    // Companies
    private IRepository<Company>? _companies;
    private IRepository<CompanyRegistrationRequest>? _companyRegistrationRequests;

    // Profiles
    private IRepository<Profile>? _profiles;

    // CVs
    private IRepository<CV>? _cvs;
    private IRepository<CVSkill>? _cvSkills;
    private IRepository<CVExperience>? _cvExperiences;
    private IRepository<CVEducation>? _cvEducations;
    private IRepository<CVCertificate>? _cvCertificates;

    // Jobs
    private IRepository<JobPost>? _jobPosts;
    private IRepository<JobShift>? _jobShifts;
    private IRepository<JobPostSkill>? _jobPostSkills;

    // Applications
    private IRepository<Domain.Entities.Application>? _applications;
    private IRepository<ApplicationHistory>? _applicationHistories;
    private IRepository<ApplicationStatusLookup>? _applicationStatuses;

    // Files
    private IRepository<FileEntity>? _files;

    // Chat
    private IRepository<ChatConversation>? _chatConversations;
    private IRepository<ChatMessage>? _chatMessages;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    // Auth
    public IRepository<User> Users => _users ??= new GenericRepository<User>(_context);
    public IRepository<Role> Roles => _roles ??= new GenericRepository<Role>(_context);
    public IRepository<UserRole> UserRoles => _userRoles ??= new GenericRepository<UserRole>(_context);
    public IRepository<RefreshToken> RefreshTokens => _refreshTokens ??= new GenericRepository<RefreshToken>(_context);

    // Companies
    public IRepository<Company> Companies => _companies ??= new GenericRepository<Company>(_context);
    public IRepository<CompanyRegistrationRequest> CompanyRegistrationRequests => _companyRegistrationRequests ??= new GenericRepository<CompanyRegistrationRequest>(_context);

    // Profiles
    public IRepository<Profile> Profiles => _profiles ??= new GenericRepository<Profile>(_context);

    // CVs
    public IRepository<CV> CVs => _cvs ??= new GenericRepository<CV>(_context);
    public IRepository<CVSkill> CVSkills => _cvSkills ??= new GenericRepository<CVSkill>(_context);
    public IRepository<CVExperience> CVExperiences => _cvExperiences ??= new GenericRepository<CVExperience>(_context);
    public IRepository<CVEducation> CVEducations => _cvEducations ??= new GenericRepository<CVEducation>(_context);
    public IRepository<CVCertificate> CVCertificates => _cvCertificates ??= new GenericRepository<CVCertificate>(_context);

    // Jobs
    public IRepository<JobPost> JobPosts => _jobPosts ??= new GenericRepository<JobPost>(_context);
    public IRepository<JobShift> JobShifts => _jobShifts ??= new GenericRepository<JobShift>(_context);
    public IRepository<JobPostSkill> JobPostSkills => _jobPostSkills ??= new GenericRepository<JobPostSkill>(_context);

    // Applications
    public IRepository<Domain.Entities.Application> Applications => _applications ??= new GenericRepository<Domain.Entities.Application>(_context);
    public IRepository<ApplicationHistory> ApplicationHistories => _applicationHistories ??= new GenericRepository<ApplicationHistory>(_context);
    public IRepository<ApplicationStatusLookup> ApplicationStatuses => _applicationStatuses ??= new GenericRepository<ApplicationStatusLookup>(_context);

    // Files
    public IRepository<FileEntity> Files => _files ??= new GenericRepository<FileEntity>(_context);

    // Chat
    public IRepository<ChatConversation> ChatConversations => _chatConversations ??= new GenericRepository<ChatConversation>(_context);
    public IRepository<ChatMessage> ChatMessages => _chatMessages ??= new GenericRepository<ChatMessage>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
