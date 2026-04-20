using PTJ.Application.Common;
using PTJ.Application.DTOs.Profile;
using PTJ.Application.Services;
using PTJ.Domain.Entities;
using PTJ.Domain.Interfaces;

namespace PTJ.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfileService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProfileDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(id, cancellationToken);

        if (profile == null)
        {
            return Result<ProfileDto>.FailureResult("Profile not found");
        }

        var dto = await MapToDtoAsync(profile, cancellationToken);

        return Result<ProfileDto>.SuccessResult(dto);
    }

    public async Task<Result<List<ProfileDto>>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var profiles = await _unitOfWork.Profiles.FindAsync(
            p => p.UserId == userId,
            cancellationToken);

        var orderedProfiles = profiles
            .OrderByDescending(p => p.IsDefault)
            .ThenByDescending(p => p.UpdatedAt ?? p.CreatedAt)
            .ToList();

        var dtos = new List<ProfileDto>(orderedProfiles.Count);
        foreach (var profile in orderedProfiles)
        {
            dtos.Add(await MapToDtoAsync(profile, cancellationToken));
        }

        return Result<List<ProfileDto>>.SuccessResult(dtos);
    }

    public async Task<Result<ProfileDto>> GetDefaultByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Profiles.FirstOrDefaultAsync(
            p => p.UserId == userId && p.IsDefault,
            cancellationToken);

        if (profile == null)
        {
            profile = await _unitOfWork.Profiles.FirstOrDefaultAsync(
                p => p.UserId == userId,
                cancellationToken);
        }

        if (profile == null)
        {
            return Result<ProfileDto>.FailureResult("Profile not found for this user");
        }

        var dto = await MapToDtoAsync(profile, cancellationToken);

        return Result<ProfileDto>.SuccessResult(dto);
    }

    public async Task<Result<ProfileDto>> CreateAsync(int userId, ProfileDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return Result<ProfileDto>.FailureResult("User not found");
        }

        var existingProfiles = (await _unitOfWork.Profiles.FindAsync(
            p => p.UserId == userId,
            cancellationToken))
            .ToList();

        var shouldBeDefault = dto.IsDefault || existingProfiles.Count == 0;

        if (shouldBeDefault)
        {
            await ClearDefaultProfilesAsync(userId, cancellationToken);
        }

        var profile = new Profile
        {
            UserId = userId,
            Title = string.IsNullOrWhiteSpace(dto.Title) ? $"CV {existingProfiles.Count + 1}" : dto.Title.Trim(),
            TargetPosition = dto.TargetPosition,
            IsDefault = shouldBeDefault,
            FullName = dto.FullName ?? user.FullName,
            Email = dto.Email ?? user.Email,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber,
            StudentId = dto.StudentId,
            University = dto.University,
            Major = dto.Major,
            GPA = dto.GPA,
            YearOfStudy = dto.YearOfStudy,
            ExpectedGraduationDate = dto.ExpectedGraduationDate,
            ResumeUrl = dto.ResumeUrl,
            Bio = dto.Bio,
            LinkedInUrl = dto.LinkedInUrl,
            GitHubUrl = dto.GitHubUrl
        };

        await _unitOfWork.Profiles.AddAsync(profile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await ReplaceProfileDetailsAsync(profile.Id, dto, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var responseDto = await MapToDtoAsync(profile, cancellationToken);

        return Result<ProfileDto>.SuccessResult(responseDto, "Profile created successfully");
    }

    public async Task<Result<ProfileDto>> UpdateAsync(int id, int userId, ProfileDto dto, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(id, cancellationToken);
        if (profile == null)
        {
            return Result<ProfileDto>.FailureResult("Profile not found");
        }

        if (profile.UserId != userId)
        {
            return Result<ProfileDto>.FailureResult("You don't have permission to update this profile");
        }

        var shouldBeDefault = dto.IsDefault || profile.IsDefault;
        if (dto.IsDefault)
        {
            await ClearDefaultProfilesAsync(userId, cancellationToken, profile.Id);
        }
        else if (profile.IsDefault)
        {
            var hasAnotherDefault = await _unitOfWork.Profiles.FirstOrDefaultAsync(
                p => p.UserId == userId && p.Id != profile.Id && p.IsDefault,
                cancellationToken);

            shouldBeDefault = hasAnotherDefault == null;
        }

        profile.Title = string.IsNullOrWhiteSpace(dto.Title) ? profile.Title : dto.Title.Trim();
        profile.TargetPosition = dto.TargetPosition;
        profile.IsDefault = shouldBeDefault;
        profile.FullName = dto.FullName;
        profile.Email = dto.Email;
        profile.DateOfBirth = dto.DateOfBirth;
        profile.Gender = dto.Gender;
        profile.Address = dto.Address;
        profile.PhoneNumber = dto.PhoneNumber;
        profile.StudentId = dto.StudentId;
        profile.University = dto.University;
        profile.Major = dto.Major;
        profile.GPA = dto.GPA;
        profile.YearOfStudy = dto.YearOfStudy;
        profile.ExpectedGraduationDate = dto.ExpectedGraduationDate;
        profile.ResumeUrl = dto.ResumeUrl;
        profile.Bio = dto.Bio;
        profile.LinkedInUrl = dto.LinkedInUrl;
        profile.GitHubUrl = dto.GitHubUrl;

        _unitOfWork.Profiles.Update(profile);

        await ReplaceProfileDetailsAsync(profile.Id, dto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var responseDto = await MapToDtoAsync(profile, cancellationToken);
        return Result<ProfileDto>.SuccessResult(responseDto, "Profile updated successfully");
    }

    public async Task<Result> SetDefaultAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(id, cancellationToken);
        if (profile == null)
        {
            return Result.FailureResult("Profile not found");
        }

        if (profile.UserId != userId)
        {
            return Result.FailureResult("You don't have permission to update this profile");
        }

        await ClearDefaultProfilesAsync(userId, cancellationToken, profile.Id);
        profile.IsDefault = true;
        _unitOfWork.Profiles.Update(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.SuccessResult("Default profile updated successfully");
    }

    public async Task<Result> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(id, cancellationToken);

        if (profile == null)
        {
            return Result.FailureResult("Profile not found");
        }

        // Check ownership
        if (profile.UserId != userId)
        {
            return Result.FailureResult("You don't have permission to delete this profile");
        }

        var shouldPromoteAnotherDefault = profile.IsDefault;

        _unitOfWork.Profiles.Remove(profile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (shouldPromoteAnotherDefault)
        {
            var nextProfile = await _unitOfWork.Profiles.FirstOrDefaultAsync(
                p => p.UserId == userId,
                cancellationToken);

            if (nextProfile != null)
            {
                nextProfile.IsDefault = true;
                _unitOfWork.Profiles.Update(nextProfile);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        return Result.SuccessResult("Profile deleted successfully");
    }

    private async Task ClearDefaultProfilesAsync(int userId, CancellationToken cancellationToken, int? excludeProfileId = null)
    {
        var defaultProfiles = await _unitOfWork.Profiles.FindAsync(
            p => p.UserId == userId && p.IsDefault && (!excludeProfileId.HasValue || p.Id != excludeProfileId.Value),
            cancellationToken);

        foreach (var existingDefault in defaultProfiles)
        {
            existingDefault.IsDefault = false;
            _unitOfWork.Profiles.Update(existingDefault);
        }
    }

    private async Task ReplaceProfileDetailsAsync(int profileId, ProfileDto dto, CancellationToken cancellationToken)
    {
        var oldSkills = await _unitOfWork.ProfileSkills.FindAsync(s => s.ProfileId == profileId, cancellationToken);
        _unitOfWork.ProfileSkills.RemoveRange(oldSkills);

        foreach (var skillDto in dto.Skills)
        {
            var skill = new ProfileSkill
            {
                ProfileId = profileId,
                SkillName = skillDto.SkillName,
                ProficiencyLevel = skillDto.ProficiencyLevel,
                YearsOfExperience = skillDto.YearsOfExperience
            };

            await _unitOfWork.ProfileSkills.AddAsync(skill, cancellationToken);
        }

        var oldExperiences = await _unitOfWork.ProfileExperiences.FindAsync(e => e.ProfileId == profileId, cancellationToken);
        _unitOfWork.ProfileExperiences.RemoveRange(oldExperiences);

        foreach (var expDto in dto.Experiences)
        {
            var experience = new ProfileExperience
            {
                ProfileId = profileId,
                CompanyName = expDto.CompanyName,
                Position = expDto.Position,
                Description = expDto.Description,
                StartDate = expDto.StartDate,
                EndDate = expDto.EndDate,
                IsCurrentlyWorking = expDto.IsCurrentlyWorking
            };

            await _unitOfWork.ProfileExperiences.AddAsync(experience, cancellationToken);
        }

        var oldEducations = await _unitOfWork.ProfileEducations.FindAsync(e => e.ProfileId == profileId, cancellationToken);
        _unitOfWork.ProfileEducations.RemoveRange(oldEducations);

        foreach (var eduDto in dto.Educations)
        {
            var education = new ProfileEducation
            {
                ProfileId = profileId,
                InstitutionName = eduDto.InstitutionName,
                Degree = eduDto.Degree,
                FieldOfStudy = eduDto.FieldOfStudy,
                StartDate = eduDto.StartDate,
                EndDate = eduDto.EndDate,
                GPA = eduDto.GPA,
                Description = eduDto.Description
            };

            await _unitOfWork.ProfileEducations.AddAsync(education, cancellationToken);
        }

        var oldCertificates = await _unitOfWork.ProfileCertificates.FindAsync(c => c.ProfileId == profileId, cancellationToken);
        _unitOfWork.ProfileCertificates.RemoveRange(oldCertificates);

        foreach (var certDto in dto.Certificates)
        {
            var certificate = new ProfileCertificate
            {
                ProfileId = profileId,
                Name = certDto.Name,
                IssuingOrganization = certDto.IssuingOrganization,
                IssueDate = certDto.IssueDate,
                ExpiryDate = certDto.ExpiryDate,
                CredentialId = certDto.CredentialId,
                CredentialUrl = certDto.CredentialUrl,
                CertificateFileUrl = certDto.CertificateFileUrl
            };

            await _unitOfWork.ProfileCertificates.AddAsync(certificate, cancellationToken);
        }
    }

    private async Task<ProfileDto> MapToDtoAsync(Profile profile, CancellationToken cancellationToken)
    {
        var skills = await _unitOfWork.ProfileSkills.FindAsync(s => s.ProfileId == profile.Id, cancellationToken);
        var experiences = await _unitOfWork.ProfileExperiences.FindAsync(e => e.ProfileId == profile.Id, cancellationToken);
        var educations = await _unitOfWork.ProfileEducations.FindAsync(e => e.ProfileId == profile.Id, cancellationToken);
        var certificates = await _unitOfWork.ProfileCertificates.FindAsync(c => c.ProfileId == profile.Id, cancellationToken);

        return new ProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            Title = profile.Title,
            TargetPosition = profile.TargetPosition,
            IsDefault = profile.IsDefault,
            FullName = profile.FullName,
            Email = profile.Email,
            DateOfBirth = profile.DateOfBirth,
            Gender = profile.Gender,
            Address = profile.Address,
            PhoneNumber = profile.PhoneNumber,
            StudentId = profile.StudentId,
            University = profile.University,
            Major = profile.Major,
            GPA = profile.GPA,
            YearOfStudy = profile.YearOfStudy,
            ExpectedGraduationDate = profile.ExpectedGraduationDate,
            ResumeUrl = profile.ResumeUrl,
            Bio = profile.Bio,
            LinkedInUrl = profile.LinkedInUrl,
            GitHubUrl = profile.GitHubUrl,
            Skills = skills.Select(s => new ProfileSkillDto
            {
                Id = s.Id,
                SkillName = s.SkillName,
                ProficiencyLevel = s.ProficiencyLevel,
                YearsOfExperience = s.YearsOfExperience
            }).ToList(),
            Experiences = experiences.Select(e => new ProfileExperienceDto
            {
                Id = e.Id,
                CompanyName = e.CompanyName,
                Position = e.Position,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                IsCurrentlyWorking = e.IsCurrentlyWorking
            }).ToList(),
            Educations = educations.Select(e => new ProfileEducationDto
            {
                Id = e.Id,
                InstitutionName = e.InstitutionName,
                Degree = e.Degree,
                FieldOfStudy = e.FieldOfStudy,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                GPA = e.GPA,
                Description = e.Description
            }).ToList(),
            Certificates = certificates.Select(c => new ProfileCertificateDto
            {
                Id = c.Id,
                Name = c.Name,
                IssuingOrganization = c.IssuingOrganization,
                IssueDate = c.IssueDate,
                ExpiryDate = c.ExpiryDate,
                CredentialId = c.CredentialId,
                CredentialUrl = c.CredentialUrl,
                CertificateFileUrl = c.CertificateFileUrl
            }).ToList()
        };
    }
}
