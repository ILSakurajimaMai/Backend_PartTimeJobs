using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTJ.Application.Services;
using System.Security.Claims;

namespace PTJ.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AIChatController : ControllerBase
{
    private readonly IAIChatService _aiChatService;

    public AIChatController(IAIChatService aiChatService)
    {
        _aiChatService = aiChatService;
    }

    [HttpPost("message")]
    public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty");

        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized();
        }

        // Pass cancellation token
        var result = await _aiChatService.ChatAsync(userId, request.Message, HttpContext.RequestAborted);

        if (!result.Success)
            return BadRequest(new { errors = result.Message ?? "Unknown error" }); 

        return Ok(new { response = result.Data });
    }

    [HttpPost("restart")]
    public async Task<IActionResult> RestartConversation()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized();
        }

        var result = await _aiChatService.RestartSessionAsync(userId, HttpContext.RequestAborted);

        if (!result.Success)
            return BadRequest(new { errors = result.Message ?? "Failed to restart conversation" });

        return Ok(new { message = result.Message });
    }
}

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
}
