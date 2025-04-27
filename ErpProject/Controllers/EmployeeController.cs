using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("employee")]
public class EmployeeController: Controller
{
    private readonly EmployeeServices _service;

    public EmployeeController(EmployeeServices service)
    {
        _service = service;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        List<Employee> employees = await _service.GetAllEmployeesAsync();

        if(employees is null)
        {
            return NotFound("The list of Employees is null");
        }

        return View(employees);
    }
}
