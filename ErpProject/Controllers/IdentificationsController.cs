using System.Security.Claims;
using System.Threading.Tasks;
using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Authorize]
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
        if (id <= 0)
        {
            return PartialView();
        }

        if (User.FindFirst(ClaimTypes.Role)?.Value == "Employee" && id != Convert.ToInt32(User.FindFirst("UserId")?.Value))
        {
            Identifications realIdentifications = await _services.GetIdentificationsAsync(Convert.ToInt32(User.FindFirst("UserId")?.Value));

            return PartialView(realIdentifications);
        }

        Identifications identifications = await _services.GetIdentificationsAsync(id);

        return PartialView(identifications);
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("register/{id}")]
    public async Task<IActionResult> Register(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        Identifications identifications = new Identifications();

        identifications.EmployeeId = id;

        return PartialView(identifications);
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("register/{id}")]
    public async Task<IActionResult> Register(int id, Identifications identifications)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Employee", new { id });
        }

        if (id <= 0)
        {
            return NotFound();
        }

        bool result = await _services.AddIdentificationsAsync(id, identifications);

        if (!result)
        {
            return RedirectToAction("Details", "Employee", new { id });
        }

        return RedirectToAction("Details", "Employee", new { id });
    }

    [HttpGet("edit/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public IActionResult Edit(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        Identifications identifications = new Identifications();

        identifications.EmployeeId = id;

        return PartialView(identifications);
    }

    [HttpPost("edit/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Identifications identifications)
    {
        if (id <= 0 || identifications is null)
        {
            return NotFound();
        }

        bool result = await _services.EditIdentificationsAsync(id, identifications);

        if (!result)
        {
            //return RedirectToAction("Details", "Employee", new { id });
            return NotFound();
        }

        return RedirectToAction("Details", "Employee", new { id });
    }
}
