using Employees.Api.Mapping.Employees;
using Employees.Contracts.Employee;
using Employees.Core.IServices;
using Employees.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeesServices _services;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeesServices services, ILogger<EmployeesController> logger)
    {
        _services = services;
        _logger = logger;
    }

    [HttpGet(Endpoint.Employees.Index)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet(Endpoint.Employees.GetAllEmployees)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Get All Employees cancelled by client");
            cancellationToken.ThrowIfCancellationRequested();
        }

        List<ResponseEmployee.Get> responseList = new List<ResponseEmployee.Get>();

        await foreach (Employee employee in _services.GetAllAsync(cancellationToken))
        {
            ResponseEmployee.Get response = employee.MapToGetResponse();
            responseList.Add(response);
        }

        return Json(responseList);
    }

    [HttpGet(Endpoint.Employees.Create)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost(Endpoint.Employees.Create)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RequestEmployee.Create request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        bool emailExists = await _services.EmailExistsAsync(request.Email);

        if (emailExists)
        {
            ModelState.AddModelError(nameof(request.Email), "Email already exists");
            return View(request);
        }

        Employee employee = await request.MapToCreateRequest();

        int employeeId = await _services.CreateAsync(employee, cancellationToken);

        if (employeeId <= 0)
        {
            _logger.LogWarning("Employee was not created. Id was 0 or smaller");
            return NotFound();
        }

        _logger.LogInformation($"Employee with Id: {employeeId} is created");

        return RedirectToAction("Create", "Credentials", new { id = employeeId });
    }
}
