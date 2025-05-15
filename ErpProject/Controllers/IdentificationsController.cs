using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("identifications")]
public class IdentificationsController : Controller
{
    private readonly IdentificationsServices _services;

    public IdentificationsController(IdentificationsServices services)
    {
        _services = services;
    }

    [HttpGet("index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        if(id <= 0)
        {
            return View();
        }

        Identifications identifications = await _services.GetIdentificationsAsync(id);

        if(identifications.Employee_Id <= 0)
        {
            identifications.Employee_Id = id;
        }

        return PartialView(identifications);
    }
}
