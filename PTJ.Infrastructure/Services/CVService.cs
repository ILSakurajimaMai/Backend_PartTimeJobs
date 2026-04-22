using PTJ.Application.Common;
using PTJ.Application.DTOs.CV;
using PTJ.Application.Services;
using PTJ.Domain.Entities;
using PTJ.Domain.Interfaces;

namespace PTJ.Infrastructure.Services;

public class CVService : ICVService
{
    private readonly IUnitOfWork _unitOfWork;

    public CVService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CVDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cv = await _unitOfWork.CVs.GetByIdAsync(id, cancellationToken);

        if (cv == null)
        {
            return Result<CVDto>.FailureResult("CV not found");
        }

        var dto = await MapToDtoAsync(cv, cancellationToken);

        return Result<CVDto>.SuccessResult(dto);
    }

    public async Task<Result<List<CVDto>>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var cvs = await _unitOfWork.CVs.FindAsync(
            p => p.UserId == userId,
            cancellationToken);

        var orderedCVs = cvs
            .OrderByDescending(p => p.IsDefault)
            .ThenByDescending(p => p.UpdatedAt ?? p.CreatedAt)
            .ToList();

        var dtos = new List<CVDto>(orderedCVs.Count);
        foreach (var cv in orderedCVs)
        {
            dtos.Add(await MapToDtoAsync(cv, cancellationToken));
        }

        return Result<List<CVDto>>.SuccessResult(dtos);
    }

    public async Task<Result<CVDto>> GetDefaultByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var cv = await _unitOfWork.CVs.FirstOrDefaultAsync(
            p => p.UserId == userId && p.IsDefault,
            cancellationToken);

        if (cv == null)
        {
            cv = await _unitOfWork.CVs.FirstOrDefaultAsync(
                p => p.UserId == userId,
                cancellationToken);
        }

        if (cv == null)
        {
            return Result<CVDto>.FailureResult("CV not found for this user");
        }

        var dto = await MapToDtoAsync(cv, cancellationToken);

        return Result<CVDto>.SuccessResult(dto);
    }

    public async Task<Result<CVDto>> CreateAsync(int userId, CVDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return Result<CVDto>.FailureResult("User not found");
        }

        var existingCVs = (await _unitOfWork.CVs.FindAsync(
            p => p.UserId == userId,
            cancellationToken))
            .ToList();

        var shouldBeDefault = dto.IsDefault || existingCVs.Count == 0;

        if (shouldBeDefault)
        {
            await ClearDefaultCVsAsync(userId, cancellationToken);
        }

        var userProfile = await _unitOfWork.Profiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        var cv = new CV
        {
            UserId = userId,
            ProfileId = userProfile?.Id,
            Title = string.IsNullOrWhiteSpace(dto.Title) ? $"CV {existingCVs.Count + 1}" : dto.Title.Trim(),
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

        await _unitOfWork.CVs.AddAsync(cv, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await ReplaceCVDetailsAsync(cv.Id, dto, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var responseDto = await MapToDtoAsync(cv, cancellationToken);

        return Result<CVDto>.SuccessResult(responseDto, "CV created successfully");
    }

    public async Task<Result<CVDto>> UpdateAsync(int id, int userId, CVDto dto, CancellationToken cancellationToken = default)
    {
        var cv = await _unitOfWork.CVs.GetByIdAsync(id, cancellationToken);
        if (cv == null)
        {
            return Result<CVDto>.FailureResult("CV not found");
        }

        if (cv.UserId != userId)
        {
            return Result<CVDto>.FailureResult("You don't have permission to update this CV");
        }

        var shouldBeDefault = dto.IsDefault || cv.IsDefault;
        if (dto.IsDefault)
        {
            await ClearDefaultCVsAsync(userId, cancellationToken, cv.Id);
        }
        else if (cv.IsDefault)
        {
            var hasAnotherDefault = await _unitOfWork.CVs.FirstOrDefaultAsync(
                p => p.UserId == userId && p.Id != cv.Id && p.IsDefault,
                cancellationToken);

            shouldBeDefault = hasAnotherDefault == null;
        }

        cv.Title = string.IsNullOrWhiteSpace(dto.Title) ? cv.Title : dto.Title.Trim();
        cv.TargetPosition = dto.TargetPosition;
        cv.IsDefault = shouldBeDefault;
        cv.FullName = dto.FullName;
        cv.Email = dto.Email;
        cv.DateOfBirth = dto.DateOfBirth;
        cv.Gender = dto.Gender;
        cv.Address = dto.Address;
        cv.PhoneNumber = dto.PhoneNumber;
        cv.StudentId = dto.StudentId;
        cv.University = dto.University;
        cv.Major = dto.Major;
        cv.GPA = dto.GPA;
        cv.YearOfStudy = dto.YearOfStudy;
        cv.ExpectedGraduationDate = dto.ExpectedGraduationDate;
        cv.ResumeUrl = dto.ResumeUrl;
        cv.Bio = dto.Bio;
        cv.LinkedInUrl = dto.LinkedInUrl;
        cv.GitHubUrl = dto.GitHubUrl;

        _unitOfWork.CVs.Update(cv);

        await ReplaceCVDetailsAsync(cv.Id, dto, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var responseDto = await MapToDtoAsync(cv, cancellationToken);
        return Result<CVDto>.SuccessResult(responseDto, "CV updated successfully");
    }

    public async Task<Result> SetDefaultAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var cv = await _unitOfWork.CVs.GetByIdAsync(id, cancellationToken);
        if (cv == null)
        {
            return Result.FailureResult("CV not found");
        }

        if (cv.UserId != userId)
        {
            return Result.FailureResult("You don't have permission to update this CV");
        }

        await ClearDefaultCVsAsync(userId, cancellationToken, cv.Id);
        cv.IsDefault = true;
        _unitOfWork.CVs.Update(cv);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.SuccessResult("Default CV updated successfully");
    }

    public async Task<Result> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        var cv = await _unitOfWork.CVs.GetByIdAsync(id, cancellationToken);

        if (cv == null)
        {
            return Result.FailureResult("CV not found");
        }

        if (cv.UserId != userId)
        {
            return Result.FailureResult("You don't have permission to delete this CV");
        }

        var shouldPromoteAnotherDefault = cv.IsDefault;

        _unitOfWork.CVs.Remove(cv);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (shouldPromoteAnotherDefault)
        {
            var nextCV = await _unitOfWork.CVs.FirstOrDefaultAsync(
                p => p.UserId == userId,
                cancellationToken);

            if (nextCV != null)
            {
                nextCV.IsDefault = true;
                _unitOfWork.CVs.Update(nextCV);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        return Result.SuccessResult("CV deleted successfully");
    }

    private async Task ClearDefaultCVsAsync(int userId, CancellationToken cancellationToken, int? excludeCVId = null)
    {
        var defaultCVs = await _unitOfWork.CVs.FindAsync(
            p => p.UserId == userId && p.IsDefault && (!excludeCVId.HasValue || p.Id != excludeCVId.Value),
            cancellationToken);

        foreach (var existingDefault in defaultCVs)
        {
            existingDefault.IsDefault = false;
            _unitOfWork.CVs.Update(existingDefault);
        }
    }

    private async Task ReplaceCVDetailsAsync(int cvId, CVDto dto, CancellationToken cancellationToken)
    {
        var oldSkills = await _unitOfWork.CVSkills.FindAsync(s => s.ProfileId == cvId, cancellationToken);
        _unitOfWork.CVSkills.RemoveRange(oldSkills);

        foreach (var skillDto in dto.Skills)
        {
            var skill = new CVSkill
            {
                ProfileId = cvId,
                SkillName = skillDto.SkillName,
                ProficiencyLevel = skillDto.ProficiencyLevel,
                YearsOfExperience = skillDto.YearsOfExperience
            };

            await _unitOfWork.CVSkills.AddAsync(skill, cancellationToken);
        }

        var oldExperiences = await _unitOfWork.CVExperiences.FindAsync(e => e.ProfileId == cvId, cancellationToken);
        _unitOfWork.CVExperiences.RemoveRange(oldExperiences);

        foreach (var expDto in dto.Experiences)
        {
            var experience = new CVExperience
            {
                ProfileId = cvId,
                CompanyName = expDto.CompanyName,
                Position = expDto.Position,
                Description = expDto.Description,
                StartDate = expDto.StartDate,
                EndDate = expDto.EndDate,
                IsCurrentlyWorking = expDto.IsCurrentlyWorking
            };

            await _unitOfWork.CVExperiences.AddAsync(experience, cancellationToken);
        }

        var oldEducations = await _unitOfWork.CVEducations.FindAsync(e => e.ProfileId == cvId, cancellationToken);
        _unitOfWork.CVEducations.RemoveRange(oldEducations);

        foreach (var eduDto in dto.Educations)
        {
            var education = new CVEducation
            {
                ProfileId = cvId,
                InstitutionName = eduDto.InstitutionName,
                Degree = eduDto.Degree,
                FieldOfStudy = eduDto.FieldOfStudy,
                StartDate = eduDto.StartDate,
                EndDate = eduDto.EndDate,
                GPA = eduDto.GPA,
                Description = eduDto.Description
            };

            await _unitOfWork.CVEducations.AddAsync(education, cancellationToken);
        }

        var oldCertificates = await _unitOfWork.CVCertificates.FindAsync(c => c.ProfileId == cvId, cancellationToken);
        _unitOfWork.CVCertificates.RemoveRange(oldCertificates);

        foreach (var certDto in dto.Certificates)
        {
            var certificate = new CVCertificate
            {
                ProfileId = cvId,
                Name = certDto.Name,
                IssuingOrganization = certDto.IssuingOrganization,
                IssueDate = certDto.IssueDate,
                ExpiryDate = certDto.ExpiryDate,
                CredentialId = certDto.CredentialId,
                CredentialUrl = certDto.CredentialUrl,
                CertificateFileUrl = certDto.CertificateFileUrl
            };

            await _unitOfWork.CVCertificates.AddAsync(certificate, cancellationToken);
        }
    }

    private async Task<CVDto> MapToDtoAsync(CV cv, CancellationToken cancellationToken)
    {
        var skills = await _unitOfWork.CVSkills.FindAsync(s => s.ProfileId == cv.Id, cancellationToken);
        var experiences = await _unitOfWork.CVExperiences.FindAsync(e => e.ProfileId == cv.Id, cancellationToken);
        var educations = await _unitOfWork.CVEducations.FindAsync(e => e.ProfileId == cv.Id, cancellationToken);
        var certificates = await _unitOfWork.CVCertificates.FindAsync(c => c.ProfileId == cv.Id, cancellationToken);

        return new CVDto
        {
            Id = cv.Id,
            UserId = cv.UserId,
            Title = cv.Title,
            TargetPosition = cv.TargetPosition,
            IsDefault = cv.IsDefault,
            FullName = cv.FullName,
            Email = cv.Email,
            DateOfBirth = cv.DateOfBirth,
            Gender = cv.Gender,
            Address = cv.Address,
            PhoneNumber = cv.PhoneNumber,
            StudentId = cv.StudentId,
            University = cv.University,
            Major = cv.Major,
            GPA = cv.GPA,
            YearOfStudy = cv.YearOfStudy,
            ExpectedGraduationDate = cv.ExpectedGraduationDate,
            ResumeUrl = cv.ResumeUrl,
            Bio = cv.Bio,
            LinkedInUrl = cv.LinkedInUrl,
            GitHubUrl = cv.GitHubUrl,
            Skills = skills.Select(s => new CVSkillDto
            {
                Id = s.Id,
                SkillName = s.SkillName,
                ProficiencyLevel = s.ProficiencyLevel,
                YearsOfExperience = s.YearsOfExperience
            }).ToList(),
            Experiences = experiences.Select(e => new CVExperienceDto
            {
                Id = e.Id,
                CompanyName = e.CompanyName,
                Position = e.Position,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                IsCurrentlyWorking = e.IsCurrentlyWorking
            }).ToList(),
            Educations = educations.Select(e => new CVEducationDto
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
            Certificates = certificates.Select(c => new CVCertificateDto
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
