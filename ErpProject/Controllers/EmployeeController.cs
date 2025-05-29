using System.Security.Claims;
using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Authorize]
[Route("employee")]
public class EmployeeController : Controller
{
    private readonly EmployeeServices _service;

    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(EmployeeServices service, ILogger<EmployeeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [Route("index")]
    [Authorize(Roles = "Owner, Admin, Manager")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("getallemployees")]
    [Authorize(Roles = "Owner, Admin, Manager")]
    public async Task<IActionResult> GetAllEmployees()
    {
        List<Employee> employeesList = new List<Employee>();

        await foreach (Employee employee in _service.GetAllEmployeesAsync())
        {
            employeesList.Add(employee);
        }

        if (employeesList is null || employeesList.Count == 0)
        {
            return Json(new { message = "No employees found." });
        }

        return Json(employeesList);
    }

    [HttpGet("register")]
    [Authorize(Roles = "Admin, Manager")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Register(Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return View(employee);
        }

        bool emailExists = await _service.EmailExistsAsync(employee.Email);

        if (emailExists)
        {
            ModelState.AddModelError("Email", "Email already exists. Please try another.");
            return View(employee);
        }

        int id = await _service.AddEmployeeAsync(employee);

        if (id <= 0)
        {
            ModelState.AddModelError(string.Empty, "An unexpected error has occurred while saving. Please try again.");
            return View(employee);
        }

        return RedirectToAction("Index", "Credentials", new { id });
    }

    [HttpGet("details/{id}")]
    public IActionResult Details(int id)
    {
        if (User.FindFirst(ClaimTypes.Role)?.Value == "Employee" && id != Convert.ToInt32(User.FindFirst("UserId")?.Value))
        {
            EmployeeId realId = new EmployeeId(Convert.ToInt32(User.FindFirst("UserId")?.Value));

            return View(realId);
        }
        
        EmployeeId newId = new EmployeeId(id);

        return View(newId);
    }

    [HttpGet("maindetails/{id}")]
    public async Task<IActionResult> MainDetails(int id)
    {
        if (id <= 0)
        {
            return NotFound("No employee was found");
        }

        if (User.FindFirst(ClaimTypes.Role)?.Value == "Employee" && id != Convert.ToInt32(User.FindFirst("UserId")?.Value))
        {
            Employee realEmployee = await _service.GetEmployeeByIdAsync(Convert.ToInt32(User.FindFirst("UserId")?.Value));    
            return PartialView(realEmployee);   
        }

        Employee employee = await _service.GetEmployeeByIdAsync(id);
        return PartialView(employee);
    }

    [HttpGet("delete/{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return NotFound("No Id was found");
        }

        Employee employee = await _service.GetEmployeeByIdAsync(id);

        employee.Id = id;

        if (employee is null)
        {
            ModelState.AddModelError(string.Empty, "No employee was found.");

            return View("Index");
        }

        return View(employee);
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        bool result = await _service.DeleteEmployeeByIdAsync(id);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "There was a problem with the deletion of the employee");
        }

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        if (id <= 0)
        {
            return NotFound("No employee was found");
        }

        Employee employee = await _service.GetEmployeeByIdAsync(id);

        employee.Email = string.Empty;

        employee.PhoneNumber = string.Empty;

        return PartialView(employee);
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid employee data");
        }

        if (!ModelState.IsValid)
        {
            return PartialView("Edit", employee);
        }

        bool result = await _service.EditEmployeeAsync(id, employee);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "There was aproblem while saving the new changes");
            return PartialView("Index", employee);
        }

        return PartialView("Details", employee);
    }
}
