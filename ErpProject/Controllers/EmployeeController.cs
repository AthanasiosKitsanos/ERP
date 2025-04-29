using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Employee employee)
    {
        if(!ModelState.IsValid)
        {
            return View(employee);
        }

        bool emailExists = await _service.EmailExistsAsync(employee.Email);

        if(emailExists)
        {
            ModelState.AddModelError("Email", "Email already exists. Please try another.");
            return View(employee);
        }

        int id = await _service.AddEmployeeAsync(employee);

        if(id <= 0)
        {
            ModelState.AddModelError(string.Empty, "An unexpected error has occurred while saving. Please try again.");
            return View(employee);
        }

        return RedirectToAction("Index", "Credentials", new {id});
    }

    [HttpGet("details/id")]
    public async Task<IActionResult> Details(int id)
    {
        if(!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        Employee employee = await _service.GetEmployeeByIdAsync(id);

        if(employee is null)
        {
            ModelState.AddModelError(string.Empty, "Something went wrong while retrieving the employee data");
            return RedirectToAction("Index");
        }

        return View(employee);
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if(id <= 0)
        {
            return NotFound("No Id was found");
        }

        Employee employee = await _service.GetEmployeeByIdAsync(id);

        employee.Id = id;

        if(employee is null)
        {
            ModelState.AddModelError(string.Empty, "No employee was found.");
            
            return View("Index");
        }

        return View(employee);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        bool result =  await _service.DeleteEmployeeByIdAsync(id);

        if(!result)
        {
            ModelState.AddModelError(string.Empty, "There was a problem with the deletion of the employee");
        }

        return RedirectToAction("Index");
    }
}
