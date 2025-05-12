using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

public class LogInController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
