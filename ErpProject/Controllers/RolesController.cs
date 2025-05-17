using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

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

        RoleName roleName = new RoleName(await _services.GetEmployeeRoleAsync(id));

        return PartialView(roleName);
    }

    [HttpGet("edit/{id}")]
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

        return PartialView(roles);
    }
}
