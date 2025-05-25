using ErpProject.JsonWebToken;
using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("login")]
public class LogInController : Controller
{
    private readonly LogInServices _services;
    private readonly JWTServices _jwtServices;

    public LogInController(LogInServices services, JWTServices jwtServices)
    {
        _services = services;
        _jwtServices = jwtServices;
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

        LoggedInData data = await _services.GetLoggedInDataAsync(login.Username);

        string token = _jwtServices.CreateJWToken(data, login.RememberMe);

        HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(login.RememberMe ? 720 : 1)
        });

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LogOut()
    {
        Response.Cookies.Delete("jwt");

        return RedirectToAction("Index", "LogIn");
    }
}
