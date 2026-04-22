using PTJ.Application.Common;
using PTJ.Application.DTOs.Profile;

namespace PTJ.Application.Services;

public interface IProfileService
{
    Task<Result<ProfileDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<ProfileDto>> UpdateAsync(int userId, UpdateProfileDto dto, CancellationToken cancellationToken = default);
}
