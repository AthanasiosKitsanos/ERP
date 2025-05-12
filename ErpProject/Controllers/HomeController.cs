using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

public class HomeController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
