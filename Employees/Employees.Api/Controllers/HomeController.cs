using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class HomeController : Controller
{
    [HttpGet(Endpoint.Home.Index)]
    public IActionResult HomePage()
    {
        return View();
    }
}
