using System;
using Microsoft.AspNetCore.Mvc;
using ErpProject.Models.DTOModels.EmployeeDTO;
using ErpProject.Models.EmployeeProfile;
using ErpProject.Interfaces;


namespace ErpProject.Controllers;

public class EmployeeController: Controller
{
    public readonly IEmployeeService _empService;

    public EmployeeController(IEmployeeService empService)
    {
        _empService = empService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var employeeList = await _empService.GetEmployeesAsync();

        if(employeeList is null)
        {
            return NotFound();
        }

        return View(employeeList);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var employee = await _empService.GetEmployeeByIdAsync(id);

        if(employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Register(EmployeeDTO newEmployee)
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
