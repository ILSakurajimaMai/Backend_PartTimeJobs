using PTJ.Application.Common;
using PTJ.Application.DTOs.Profile;

namespace PTJ.Application.Services;

public interface IProfileService
{
    Task<Result<ProfileDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<List<ProfileDto>>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<ProfileDto>> GetDefaultByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<ProfileDto>> CreateAsync(int userId, ProfileDto dto, CancellationToken cancellationToken = default);
    Task<Result<ProfileDto>> UpdateAsync(int id, int userId, ProfileDto dto, CancellationToken cancellationToken = default);
    Task<Result> SetDefaultAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default);
}
