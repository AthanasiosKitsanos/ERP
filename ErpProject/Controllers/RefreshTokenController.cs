using ErpProject.JsonWebToken;
using ErpProject.Models;
using ErpProject.RefreshTokens;
using ErpProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

public class RefreshTokenController : Controller
{
    private readonly RefreshTokenServices _services;
    private readonly LogInServices _logInServices;
    private readonly JWTServices _jwtServices;

    public RefreshTokenController(RefreshTokenServices services, LogInServices logInServices, JWTServices jwtServices)
    {
        _services = services;
        _logInServices = logInServices;
        _jwtServices = jwtServices;
    }

    [Authorize]
    [HttpGet("api/refreshtoken")]
    public IActionResult Get()
    {
        return Ok("Access token is valid");
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshAccessToken()
    {
        if (!Request.Cookies.TryGetValue("refreshtoken", out string? oldToken) || string.IsNullOrEmpty(oldToken))
        {
            return Unauthorized();
        }

        int employeeId = await _services.ValidateRefreshTokenAsync(oldToken);

        if (employeeId <= 0)
        {
            return Unauthorized();
        }

        string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        if (!await _services.RevokeRefreshTokenAsync(oldToken, ipAddress))
        {
            return StatusCode(500, "Failed to Revoke the old Refresh Token");
        }

        Response.Cookies.Delete("refreshtoken");
        Response.Cookies.Delete("jwt");

        if (!await _services.GenerateRefreshTokenAsync(employeeId, ipAddress, rememberMe: true))
        {
            return StatusCode(500, "Failed to create a refresh token");
        }

        LoggedInData data = await _logInServices.GetLoggedInDataByIdAsync(employeeId);

        string newRefreshToken = await _services.GetRefreshTokenAsync(employeeId);

        string newAccessToken = _jwtServices.CreateJWToken(data);

        Response.Cookies.Append("jwt", newAccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(5)
        });

        Response.Cookies.Append("refreshtoken", newRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok();
    }
}
