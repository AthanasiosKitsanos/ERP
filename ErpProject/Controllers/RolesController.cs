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

    [HttpGet("index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        AddRoleViewModel model = new AddRoleViewModel
        {
            EmployeeId = id,
            Roles = await _roleService.GetAllRolesAsync()
        };

        if(model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("index/{id}")]
    public async Task<IActionResult> AddRole(AddRoleViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return RedirectToAction("Index", model.EmployeeId);
        }

        var result = await _roleService.AddRoleToEmployeeAsync(model.EmployeeId, model.SelectedRole);

        if(!result)
        {
            return RedirectToAction("Index", model.EmployeeId);
        }

        return RedirectToAction("Index", "Employee");
    }
}

