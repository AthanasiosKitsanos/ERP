using Microsoft.AspNetCore.Mvc;
using Employees.Shared.CustomEndpoints;

namespace Employees.Api.Controllers;

public class HomeController : Controller
{
    [HttpGet(Endpoints.Home.Index)]
    public IActionResult HomePage()
    {
        return View();
    }
}
