using System;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

public class HomeController: Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
