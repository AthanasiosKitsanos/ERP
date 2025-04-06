using System;
using Microsoft.AspNetCore.Mvc;
using ErpProject.Models.EmployeeModel;
using ErpProject.Services.EmployeeServices;
using System.Threading.Tasks;
using ErpProject.Models.DTOModels.Update;


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
    public async Task<IActionResult> Register(Employee newEmployee)
    {
        if(!ModelState.IsValid)
        {
            return View("Register", newEmployee);
        }

        var result = await _employeeService.RegisterNewEmployeeAsync(newEmployee);

        if(!result)
        {
            return View(newEmployee);
        }

        int newEmployeeId = await _employeeService.GetIdFromEmployeeAsync(newEmployee.Email);

        //return RedirectToAction("Index");
        return RedirectToAction("Index", "Roles", new {id = newEmployeeId});
    }

    [HttpGet("update/{id}")]
    public IActionResult Update(int id)
    {
        UpdateDTO update = new UpdateDTO();

        if(update is null)
        {
            return NotFound();
        }

        update.Id = id;

        return View(update);
    }

    [HttpPost("update/{id}")]
    public async Task<IActionResult> UpdateEmployee(UpdateDTO dto)
    {
        if(!ModelState.IsValid)
        {
            return View("Update", dto);
        }

        var result = await _employeeService.UpdateEmployeeAsync(dto, dto.Id);

        if(!result)
        {
            ModelState.AddModelError("", "No changes were made. Check if the employee exists.");
            return View("Update", dto);
        }

        return RedirectToAction("Index");
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {   
        Employee employee = await _employeeService.GetEmployeeByIdAsync(id);

        if(employee is null)
        {
            return RedirectToAction("Index");
        }

        return View(employee);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeService.DeleteEmployeeAsync(id);

        if(!result)
        {
            return NotFound("There was a problem deleting the employee");
        }

        return RedirectToAction("Index");
    }
    
}
