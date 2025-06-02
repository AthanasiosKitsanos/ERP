using ErpProject.JsonWebToken;
using ErpProject.Models;
using ErpProject.RefreshTokens;
using ErpProject.RefreshTokens.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("login")]
public class LogInController : Controller
{
    private readonly LogInServices _services;
    private readonly JWTServices _jwtServices;
    private readonly RefreshTokenServices _refreshServices;

    public LogInController(LogInServices services, JWTServices jwtServices, RefreshTokenServices refreshServices)
    {
        _services = services;
        _jwtServices = jwtServices;
        _refreshServices = refreshServices;
    }

    [HttpGet("index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("index")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LogIn login)
    {   
        if (!ModelState.IsValid)
            {
                return View();
            }

        bool logInSuccess = await _services.LogInAsync(login);

        if (!logInSuccess)
        {
            ModelState.AddModelError(string.Empty, "Username or Password is not valid, please try again");
            return View();
        }

        LoggedInData data = await _services.GetLoggedInDataByUsernameAsync(login.Username);

        if (data is null)
        {
            return NotFound("No data recieved");
        }

        RequestToken request = new RequestToken();

        request.AccessToken =  _jwtServices.CreateJWToken(data);

        string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString()?? "Unkown";

        bool refreshTokenSuccess = await _refreshServices.GenerateRefreshTokenAsync(data.Id, ipAddress, login.RememberMe);

        if (!refreshTokenSuccess)
        {
            ModelState.AddModelError(string.Empty, "There was problem while logging you in");
            return View();
        }

        request.RefreshToken = await _refreshServices.GetRefreshTokenAsync(data.Id);

        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            ModelState.AddModelError(string.Empty, "No token was found to log you in");
            return View();
        }

        HttpContext.Response.Cookies.Append("jwt", request.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(5)
        });

        HttpContext.Response.Cookies.Append("refreshtoken", request.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = login.RememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(15) 
        });

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LogOut()
    {
        Response.Cookies.Delete("jwt");
        Response.Cookies.Delete("refreshtoken");

        return RedirectToAction("Index", "LogIn");
    }
}
