using PTJ.Application.Common;
using PTJ.Application.DTOs.CV;

namespace PTJ.Application.Services;

public interface ICVService
{
    Task<Result<CVDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<List<CVDto>>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<CVDto>> GetDefaultByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Result<CVDto>> CreateAsync(int userId, CVDto dto, CancellationToken cancellationToken = default);
    Task<Result<CVDto>> UpdateAsync(int id, int userId, CVDto dto, CancellationToken cancellationToken = default);
    Task<Result> SetDefaultAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default);
}
