using System;
using Microsoft.AspNetCore.Mvc;
using ErpProject.Models.DTOModels.EmployeeDTO;
using ErpProject.Models.EmployeeProfile;
using ErpProject.Services;
using ErpProject.Services.EmployeeServices;


namespace ErpProject.Controllers;

[ApiController]
[Route("employee")]
public class EmployeeController: Controller
{
    public readonly EmployeeService _empService;

    public EmployeeController(EmployeeService empService)
    {
        _empService = empService;
    }

    [HttpGet("details")]
    public async Task<IActionResult> DetailsAsync()
    {
        var employeeList = await _empService.GetEmployeesAsync();

        if(employeeList is null)
        {
            return NotFound();
        }

        return View(employeeList);
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> GetEmployeeDetailsAsync(int id)
    {
        var employee = await _empService.GetEmployeeByIdAsync(id);

        if(employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewEmployeeAsync(EmployeeDTO newEmployee)
    {
        if(!ModelState.IsValid)
        {
            return View("Register", newEmployee);
        }

        var result = await _empService.RegisterNewEmployeeAsync(newEmployee);

        if(!result)
        {
            return View("Error");
        }

        return View("New Registration added.", newEmployee);
    }
}
