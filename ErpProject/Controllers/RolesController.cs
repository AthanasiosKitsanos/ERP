using ErpProject.Models.DTOModels;
using ErpProject.Services.RoleServices;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("roles")]
public class RolesController: Controller 
{
    private readonly RoleService _roleService;

    public RolesController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("add")]
    public async Task<IActionResult> Add(ViewModelDTO model)
    {
        if(model is null)
        {
            return NotFound();
        }

        model.Roles.RolesNameList = await _roleService.GetAllRolesAsync();

        if(model.Roles.RolesNameList.Count == 0)
        {
            return BadRequest("The was no list retrieved frome the database.");
        }

        return View(model);
    }

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public IActionResult AddRoles(ViewModelDTO model)
    {
        if(model is null)
        {
            return NotFound();
        }

        return RedirectToAction("Add", "EmployeeCredentials", model);
    }
}