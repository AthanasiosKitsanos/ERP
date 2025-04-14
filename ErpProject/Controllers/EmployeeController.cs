using Microsoft.AspNetCore.Mvc;
using ErpProject.Models.EmployeeModel;
using ErpProject.Services.EmployeeServices;
using ErpProject.Models.DTOModels;
using ErpProject.Helpers;


namespace ErpProject.Controllers;

[Route("employee")]
public class EmployeeController : Controller
{
    private readonly EmployeeService _employeeService;
    private readonly FileManagement _fileManagement;

    public EmployeeController(EmployeeService employeeService, FileManagement fileManagement)
    {
        _employeeService = employeeService;
        _fileManagement = fileManagement;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        List<Employee> employees = await _employeeService.GetEmployeesAsync();

        if (employees is null)
        {
            return View();
        }

        return View(employees);
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        ViewModelDTO model = new ViewModelDTO();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return View(model);
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(ViewModelDTO model)
    {
        if (model is null)
        {
            return RedirectToAction("Register");
        }
        
        var result =  await _fileManagement.UploadPhotoAsync(model.ProfilePhoto);

        model.Employee.PhotographPath = result.ProfilePhotoUrl;

        string fullPath = result.FullPath;

        if (string.IsNullOrEmpty(model.Employee.PhotographPath) || string.IsNullOrWhiteSpace(model.Employee.PhotographPath))
        {
            await _fileManagement.DeleteFile(fullPath);
        }

        return RedirectToAction("Add", "AdditionalDetails", model);
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Employee employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        if (!ModelState.IsValid)
        {
            return View(await _employeeService.DeleteEmployeeAsync(id));
        }

        bool result = await _employeeService.DeleteEmployeeAsync(id);

        if (!result)
        {
            return View(await _employeeService.DeleteEmployeeAsync(id));
        }

        return RedirectToAction("Index");
    }
}
