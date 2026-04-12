using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PTJ.Application.Common;
using PTJ.Application.DTOs.Auth;
using PTJ.Application.Services;
using PTJ.Domain.Entities;
using PTJ.Domain.Interfaces;

namespace PTJ.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly JwtOptions _jwtOptions;

    public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, IOptions<JwtOptions> jwtOptions)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        // Check if email already exists
        var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == dto.Email,
            cancellationToken);

        if (existingUser != null)
        {
            return Result<RegisterResponseDto>.FailureResult("Email already exists");
        }

        // Get the requested role
        var upperRole = dto.Role.ToUpper();
        if (upperRole != "STUDENT" && upperRole != "EMPLOYER")
        {
            return Result<RegisterResponseDto>.FailureResult("Invalid role specified");
        }

        var selectedRole = await _unitOfWork.Roles.FirstOrDefaultAsync(
            r => r.Name == upperRole,
            cancellationToken);

        if (selectedRole == null)
        {
            return Result<RegisterResponseDto>.FailureResult($"{upperRole} role not found in system");
        }

        // Create user
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber,
            IsEmailVerified = false,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Assign the selected role
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = selectedRole.Id,
            AssignedAt = DateTime.UtcNow
        };

        await _unitOfWork.UserRoles.AddAsync(userRole, cancellationToken);

        // Create Profile or Company automatically based on role
        if (upperRole == "STUDENT")
        {
            var profile = new Profile
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Profiles.AddAsync(profile, cancellationToken);
        }
        else if (upperRole == "EMPLOYER")
        {
            var company = new Company
            {
                OwnerId = user.Id,
                Name = string.IsNullOrWhiteSpace(user.FullName) ? user.Email : user.FullName,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Companies.AddAsync(company, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new RegisterResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = selectedRole.Name
        };

        return Result<RegisterResponseDto>.SuccessResult(response, "Registration successful. Please login to continue.");
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, string ipAddress, CancellationToken cancellationToken = default)
    {
        // Find user by email
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == dto.Email,
            cancellationToken);

        if (user == null)
        {
            return Result<AuthResponseDto>.FailureResult("Invalid email or password");
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Result<AuthResponseDto>.FailureResult("Invalid email or password");
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return Result<AuthResponseDto>.FailureResult("Account is deactivated");
        }

        // Get user roles
        var userRoles = await _unitOfWork.UserRoles.FindAsync(
            ur => ur.UserId == user.Id,
            cancellationToken);

        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        // Optimize: Fetch all roles in one query
        var rolesEntities = await _unitOfWork.Roles.FindAsync(
            r => roleIds.Contains(r.Id),
            cancellationToken);

        var roles = rolesEntities.Select(r => r.Name).ToList();

        // Generate tokens
        var accessToken = _jwtService.GenerateAccessToken(user, roles);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays),
            CreatedByIp = ipAddress,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresMinutes)
        };

        return Result<AuthResponseDto>.SuccessResult(response, "Login successful");
    }

    public async Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken, string ipAddress, CancellationToken cancellationToken = default)
    {
        var token = await _unitOfWork.RefreshTokens.FirstOrDefaultAsync(
            rt => rt.Token == refreshToken,
            cancellationToken);

        if (token == null || !token.IsActive)
        {
            return Result<AuthResponseDto>.FailureResult("Invalid refresh token");
        }

        // Get user
        var user = await _unitOfWork.Users.GetByIdAsync(token.UserId, cancellationToken);
        if (user == null || !user.IsActive)
        {
            return Result<AuthResponseDto>.FailureResult("User not found or inactive");
        }

        // Get user roles
        var userRoles = await _unitOfWork.UserRoles.FindAsync(
            ur => ur.UserId == user.Id,
            cancellationToken);

        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        // Optimize: Fetch all roles in one query
        var rolesEntities = await _unitOfWork.Roles.FindAsync(
            r => roleIds.Contains(r.Id),
            cancellationToken);

        var roles = rolesEntities.Select(r => r.Name).ToList();

        // Generate new tokens
        var newAccessToken = _jwtService.GenerateAccessToken(user, roles);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        // Revoke old token
        token.IsRevoked = true;
        token.RevokedAt = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.ReplacedByToken = newRefreshToken;
        _unitOfWork.RefreshTokens.Update(token);

        // Save new refresh token
        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays),
            CreatedByIp = ipAddress,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresMinutes)
        };

        return Result<AuthResponseDto>.SuccessResult(response, "Token refreshed successfully");
    }

    public async Task<Result> RevokeTokenAsync(string refreshToken, int userId, string ipAddress, CancellationToken cancellationToken = default)
    {
        var token = await _unitOfWork.RefreshTokens.FirstOrDefaultAsync(
            rt => rt.Token == refreshToken && rt.UserId == userId,
            cancellationToken);

        if (token == null)
        {
            return Result.FailureResult("Invalid refresh token");
        }

        if (!token.IsActive)
        {
            return Result.FailureResult("Token already revoked or expired");
        }

        // Revoke token
        token.IsRevoked = true;
        token.RevokedAt = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        _unitOfWork.RefreshTokens.Update(token);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.SuccessResult("Logout successful");
    }
}
