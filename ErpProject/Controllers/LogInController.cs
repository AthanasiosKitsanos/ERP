using ErpProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("login")]
public class LogInController: Controller
{
    [HttpGet("index")]
    public IActionResult Index()
    {
        return View();
    }

    // [HttpPost("index")]
    // public async Task<IActionResult> Index(LogIn login)
    // {

    // } 
}
