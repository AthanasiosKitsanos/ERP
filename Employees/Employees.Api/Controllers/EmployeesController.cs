using Employees.Core.IServices;
using Employees.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeesServices _services;

    public EmployeesController(IEmployeesServices services)
    {
        _services = services;
    }

    [HttpGet(Endpoint.Employees.Index)]
    public IActionResult Index(CancellationToken cancellationToken)
    {
        return View();
    }

    [HttpGet(Endpoint.Employees.GetAllEmployees)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        List<Employee> employees = new List<Employee>();

        await foreach (Employee employee in _services.GetAllAsync(cancellationToken))
        {
            employees.Add(employee);
        }

        return Json(employees);
    }
}
