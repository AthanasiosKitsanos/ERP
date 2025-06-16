using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("home")]
public class HomeController: Controller
{
    [HttpGet("index")]
    public IActionResult Index()
    {
        return View();
    }
}
