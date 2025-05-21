using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("login")]
public class LogInController: Controller
{
    private readonly LogInServices _services;

    public LogInController(LogInServices services)
    {
        _services = services;
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
        

        return RedirectToAction("Index", "Home");
    } 
}
