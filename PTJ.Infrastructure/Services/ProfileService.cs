using PTJ.Application.Common;
using PTJ.Application.DTOs.Profile;
using PTJ.Application.Services;
using PTJ.Domain.Interfaces;

namespace PTJ.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfileService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProfileDto>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Profiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
        {
            return Result<ProfileDto>.FailureResult("Profile not found");
        }

        return Result<ProfileDto>.SuccessResult(MapToDto(profile));
    }

    public async Task<Result<ProfileDto>> UpdateAsync(int userId, UpdateProfileDto dto, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Profiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile == null)
        {
            return Result<ProfileDto>.FailureResult("Profile not found");
        }

        profile.FullName = dto.FullName;
        profile.DateOfBirth = dto.DateOfBirth;
        profile.PhoneNumber = dto.PhoneNumber;
        profile.Email = dto.Email;
        profile.Address = dto.Address;

        _unitOfWork.Profiles.Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProfileDto>.SuccessResult(MapToDto(profile), "Profile updated successfully");
    }

    private static ProfileDto MapToDto(Domain.Entities.Profile profile) => new()
    {
        Id = profile.Id,
        UserId = profile.UserId,
        FullName = profile.FullName,
        DateOfBirth = profile.DateOfBirth,
        PhoneNumber = profile.PhoneNumber,
        Email = profile.Email,
        Address = profile.Address
    };
}
