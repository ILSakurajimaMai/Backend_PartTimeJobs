using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTJ.Application.DTOs.CV;
using PTJ.Application.Services;

namespace PTJ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CVsController : ControllerBase
{
    private readonly ICVService _cvService;
    private readonly IActivityLogService _activityLogService;

    public CVsController(ICVService cvService, IActivityLogService activityLogService)
    {
        _cvService = cvService;
        _activityLogService = activityLogService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _cvService.GetByIdAsync(id, cancellationToken);

        if (!result.Success)
        {
            return NotFound(result);
        }

        var userId = GetUserId();
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var userAgent = Request.Headers["User-Agent"].ToString();
        await _activityLogService.LogActivityAsync(
            userId != 0 ? userId : null,
            "GET",
            $"/api/cvs/{id}",
            null,
            ipAddress,
            userAgent,
            200,
            0,
            $"Xem chi tiết CV ID: {id}");

        return Ok(result);
    }

    [Authorize(Roles = "STUDENT,ADMIN")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyCV(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _cvService.GetDefaultByUserIdAsync(userId, cancellationToken);

        if (!result.Success)
        {
            return NotFound(result);
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var userAgent = Request.Headers["User-Agent"].ToString();
        await _activityLogService.LogActivityAsync(
            userId,
            "GET",
            "/api/cvs/me",
            null,
            ipAddress,
            userAgent,
            200,
            0,
            "Student xem CV của mình");

        return Ok(result);
    }

    [Authorize(Roles = "STUDENT,ADMIN")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyCVs(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _cvService.GetByUserIdAsync(userId, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var userAgent = Request.Headers["User-Agent"].ToString();
        await _activityLogService.LogActivityAsync(
            userId,
            "GET",
            "/api/cvs/my",
            null,
            ipAddress,
            userAgent,
            200,
            0,
            "Student xem danh sach CV cua minh");

        return Ok(result);
    }

    [Authorize(Roles = "STUDENT,ADMIN")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CVDto dto, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _cvService.CreateAsync(userId, dto, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var userAgent = Request.Headers["User-Agent"].ToString();
        await _activityLogService.LogActivityAsync(
            userId,
            "POST",
            "/api/cvs",
            null,
            ipAddress,
            userAgent,
            200,
            0,
            "Student tao CV moi");

        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [Authorize(Roles = "STUDENT,ADMIN")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CVDto dto, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _cvService.UpdateAsync(id, userId, dto, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var userAgent = Request.Headers["User-Agent"].ToString();
        await _activityLogService.LogActivityAsync(
            userId,
            "PUT",
            $"/api/cvs/{id}",
            null,
            ipAddress,
            userAgent,
            200,
            0,
            $"Student cap nhat CV ID: {id}");

        return Ok(result);
    }

    [Authorize(Roles = "STUDENT,ADMIN")]
    [HttpPost("{id}/set-default")]
    public async Task<IActionResult> SetDefault(int id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _cvService.SetDefaultAsync(id, userId, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var userAgent = Request.Headers["User-Agent"].ToString();
        await _activityLogService.LogActivityAsync(
            userId,
            "POST",
            $"/api/cvs/{id}/set-default",
            null,
            ipAddress,
            userAgent,
            200,
            0,
            $"Student dat CV mac dinh ID: {id}");

        return Ok(result);
    }

    [Authorize(Roles = "STUDENT,ADMIN")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _cvService.DeleteAsync(id, userId, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var userAgent = Request.Headers["User-Agent"].ToString();
        await _activityLogService.LogActivityAsync(
            userId,
            "DELETE",
            $"/api/cvs/{id}",
            null,
            ipAddress,
            userAgent,
            200,
            0,
            $"Student xóa CV ID: {id}");

        return Ok(result);
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }
}
