using Application.Common;
using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("Login")]

    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await authService.LoginAsync(dto);

        return result is null
            ? Unauthorized(ApiResponse.Fail("Invalid email or password"))
            : Ok(ApiResponse<AuthResponseDto>.Ok(result, "Login successful"));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var success = await authService.RegisterAsync(dto);

        return success
            ? Ok(ApiResponse.Ok("User registered successfully"))
            : BadRequest(ApiResponse.Fail("Email already exists"));
    }
}