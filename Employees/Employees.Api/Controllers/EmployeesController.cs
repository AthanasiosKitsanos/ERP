using Employees.Api.Mapping.Employees;
using Employees.Contracts;
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

        List<ResponseEmployee> responseList = new List<ResponseEmployee>();

        await foreach (Employee employee in _services.GetAllAsync(cancellationToken))
        {
            ResponseEmployee response = employee.MapToResponse();
            responseList.Add(response);
        }

        return Json(responseList);
    }
}
