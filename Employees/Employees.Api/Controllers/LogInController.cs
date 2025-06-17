using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class LogInController : Controller
{
    [HttpGet(Endpoint.LogIn.LogInPage)]
    public IActionResult Index()
    {
        return View();
    }  
}
