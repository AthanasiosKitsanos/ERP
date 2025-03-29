using System;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

public class HomeController: Controller
{
    public ActionResult Index()
    {
        return View();
    }
}
