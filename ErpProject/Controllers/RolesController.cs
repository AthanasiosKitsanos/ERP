using System.Security.Claims;
using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Authorize]
[Route("roles")]
public class RolesController : Controller
{
    private readonly RolesServices _services;

    public RolesController(RolesServices services)
    {
        _services = services;
    }

    [HttpGet("index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        if (id <= 0)
        {
            return NotFound("There was no employee found");
        }

        if (User.FindFirst(ClaimTypes.Role)?.Value == "Employee" && id != Convert.ToInt32(User.FindFirst("UserId")?.Value))
        {
            Roles realRole = await _services.GetEmployeeRoleAsync(Convert.ToInt32(User.FindFirst("UserId")?.Value));
            return PartialView(realRole);
        }

        Roles role = await _services.GetEmployeeRoleAsync(id);

        return PartialView(role);
    }

    [HttpGet("edit/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Edit(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        RolesList roles = new RolesList();

        await foreach (Roles role in _services.GetRolesAsync())
        {
            roles.List.Add(role);
        }

        roles.EmployeeId = id;

        return PartialView(roles);
    }

    [HttpPost("edit/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RolesList role)
    {
        if (role is null)
        {
            return NotFound("No role was selected");
        }

        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Employee", new { id });
        }

        bool result = await _services.EditRoleAsync(id, role.SelectedRoleId);

        if (!result)
        {
            return PartialView("Edit", id);
        }

        return RedirectToAction("Details", "Employee", new { id });
    }

    [HttpGet("register/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Register(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        RolesList roles = new RolesList();

        await foreach (Roles role in _services.GetRolesAsync())
        {
            roles.List.Add(role);
        }

        roles.EmployeeId = id;

        return PartialView(roles);
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("register/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(int id, RolesList role)
    {
        if (role is null)
        {
            return NotFound("No role was selected");
        }

        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Employee", new { id });
        }

        bool result = await _services.AddRoleToEmployeeAsync(id, role.SelectedRoleId);

        if (!result)
        {
            return RedirectToAction("Details", "Employee", new { id });
        }

        return RedirectToAction("Details", "Employee", new { id });
    }
}
