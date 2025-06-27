using Microsoft.AspNetCore.Mvc;
using Employees.Shared.CustomEndpoints;

namespace Employees.Api.Controllers;

public class LogInController : Controller
{
    [HttpGet(Endpoints.LogIn.LogInPage)]
    public IActionResult Index()
    {
        return View();
    }  
}
