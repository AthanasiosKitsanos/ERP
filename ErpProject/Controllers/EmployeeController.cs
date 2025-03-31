using System;
using Microsoft.AspNetCore.Mvc;
using ErpProject.Models.DTOModels.EmployeeDTO;
using ErpProject.Models.EmployeeProfile;
using ErpProject.Services.EmployeeServices;


namespace ErpProject.Controllers;

[Route("employee")]
public class EmployeeController: Controller
{
    private readonly EmployeeService _employeeService;
    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        var employeeList = await _employeeService.GetEmployeesAsync();

        if(employeeList is null)
        {
            return NotFound();
        }

        return View(employeeList);
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(EmployeeDTO newEmployee)
    {
        if(!ModelState.IsValid)
        {
            return View("Register", newEmployee);
        }

        var result = await _employeeService.RegisterNewEmployeeAsync(newEmployee);

        if(!result)
        {
            return View();
        }

        return View(newEmployee);
    }

    [HttpGet("update/{id}")]
    public IActionResult Update(int id)
    {
        var employeeDTO = new EmployeeDTO();

        return View(employeeDTO);
    }

    [HttpPost("update/{id}")]
    public async Task<IActionResult> Update(EmployeeDTO dto, int id)
    {
        if(!ModelState.IsValid)
        {
            return View("Update");
        }

        var result = await _employeeService.UpdateEmployeeAsync(dto, id);

        if(!result)
        {
            return View("Error");
        }

        return RedirectToAction("Index");
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _employeeService.DeleteEmployeeAsync(id);

        if(!result)
        {
            return NotFound("There was a problem deleting the employee");
        }

        return View();
    }
    
}
