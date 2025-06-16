using System.Security.Claims;
using ErpProject.Helpers;
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

    [HttpGet("{id}/index")]
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
    [HttpGet("{id}/register")]
    public IActionResult Register(int id)
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
    [HttpPost("{id}/register")]
    public async Task<IActionResult> Register(int id, Identifications identifications)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Employees", new { id });
        }

        if (id <= 0)
        {
            return NotFound();
        }

        if (!await TinValidation.IsValidTin(identifications.TIN) || await _services.TinExistsAsync(identifications.TIN))
        {
            ModelState.AddModelError("TIN", "The TIN number is not valid");
            return RedirectToAction("Details", "Employees", new { id });
        }

        bool result = await _services.AddIdentificationsAsync(id, identifications);

        if (!result)
        {
            return RedirectToAction("Details", "Employees", new { id });
        }

        return RedirectToAction("Details", "Employees", new { id });
    }

    [HttpGet("{id}/edit")]
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

    [HttpPut("{id}/edit")]
    [Authorize(Roles = "Admin, Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Identifications identifications)
    {
        if (id <= 0 || identifications is null)
        {
            return NotFound();
        }

        if (!await TinValidation.IsValidTin(identifications.TIN) || await _services.TinExistsAsync(identifications.TIN))
        {
            ModelState.AddModelError("TIN", "The TIN number is not valid");
            return RedirectToAction("Details", "Employees", new { id });
        }

        bool result = await _services.EditIdentificationsAsync(id, identifications);

        if (!result)
        {
            return RedirectToAction("Details", "Employees", new { id });
        }

        return RedirectToAction("Details", "Employees", new { id });
    }
}
