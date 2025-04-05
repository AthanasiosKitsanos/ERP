using Microsoft.AspNetCore.Mvc;
using ErpProject.Models.RolesModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ErpProject.Services.EmployeeServices;
using ErpProject.Services.RoleServices;
using ErpProject.Models.PartialModels.AddRoleViewModel;

namespace ErpProject.Controllers;

[Route("roles")]
public class RolesController : Controller
{
    public readonly RoleService _roleService;

    public RolesController(RoleService roleService)
    {
        _roleService = roleService; 
    }

    [HttpGet("addrole/{id}")]
    public async Task<IActionResult> AddRole(int id)
    {
        var rolesList = await _roleService.GetAllRolesAsync();

        if(rolesList is null)
        {
            return View("AddRole", new AddRoleViewModel(){EmployeeId = id, Roles = rolesList!});
        }

        var model = new AddRoleViewModel
        {
            EmployeeId = id,
            Roles = rolesList
        };
        
        return View(model);
    }

    [HttpPost("addrole/{id}")]
    public async Task<IActionResult> AddRole(AddRoleViewModel model)
    {
        if(model is null)
        {
            return View("AddRole", new AddRoleViewModel());
        }

        if(!ModelState.IsValid)
        {
            return View("AddRole", model);
        }

        var result = await  _roleService.AddRoleToEmployeeAsync(model.EmployeeId, model.SelectedRole);

        if(!result)
        {
            ModelState.AddModelError("", "Failed to assign role to Employee");
            return View("Error", new {id = model.EmployeeId});
        }

        return RedirectToAction("Index", "Employee");
    }
}

