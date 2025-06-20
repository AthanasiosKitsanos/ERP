using System.Threading.Tasks;
using Azure;
using Employees.Api.Mapping.Employees;
using Employees.Contracts.Employee;
using Employees.Core.IServices;
using Employees.Domain;
using Employees.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

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

        _logger.LogInformation("List of Employees created and was sent as Json.");
        return Json(responseList);
    }

    [HttpGet(Endpoint.Employees.GetAllDetails)]
    public IActionResult Details(int id)
    {
        if (id <= 0)
        {
            Response.StatusCode = 404;
            _logger.LogWarning("Status code 404, id was not found");
            return PartialView("Error", new ErrorViewModel
            {
                StatusCode = 404,
                Message = "Id was not found"
            });
        }

        EmployeeId newId = new EmployeeId(id);

        return View(newId);
    }

    [HttpGet(Endpoint.Employees.GetMainDetails)]
    public async Task<IActionResult> GetMainDetails(int id, CancellationToken token)
    {
        Employee employee = await _services.GetByIdAsync(id, token);

        ResponseEmployee.Get response = employee.MapToGetResponse();

        if (response is null)
        {
            Response.StatusCode = 404;
            return PartialView("Error", Response.StatusCode);
        }

        return PartialView(response);
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
            _logger.LogInformation("Model State not valid, returning to Create Page again.");
            return View(request);
        }

        bool emailExists = await _services.EmailExistsAsync(request.Email);

        if (emailExists)
        {
            _logger.LogInformation($"{request.Email} already exists, returning to the Create View");
            ModelState.AddModelError(nameof(request.Email), "Email already exists");
            return View(request);
        }

        Employee employee = await request.MapToCreateEmployee();

        int employeeId = await _services.CreateAsync(employee, cancellationToken);

        if (employeeId <= 0)
        {
            _logger.LogWarning("Employee was not created. Id was 0 or smaller");
            return NotFound();
        }

        _logger.LogInformation($"Employee with Id: {employeeId} is created");

        return RedirectToAction("Create", "Credentials", new { id = employeeId });
    }

    [HttpGet(Endpoint.Employees.Update)]
    public async Task<IActionResult> Update(int id, CancellationToken token)
    {
        Employee employee = await _services.GetByIdAsync(id, token);

        RequestEmployee.Update request = employee.MapToUpdateRequest();

        if (request is null)
        {
            Response.StatusCode = 404;
            return PartialView("Error", new ErrorViewModel
            {
                StatusCode = 404,
                Message = "Page was not found"
            });
        }

        return PartialView(request);
    }

    [HttpPut(Endpoint.Employees.Update)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, RequestEmployee.Update request, CancellationToken token)
    {
        Employee employee = request.MapToUpdateEmployee();

        bool IsUpdated = await _services.UpdateAsync(id, employee, token);

        if (!IsUpdated)
        {
            _logger.LogWarning($"Employee with Id: {id}, was not updated");
            return RedirectToAction("Details", new { id });
        }

        return RedirectToAction("Details", new { id });
    }

    [HttpGet(Endpoint.Employees.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"url Id {id}");

        Employee employee = await _services.GetInfoForDeleteAysnc(id);

        ResponseEmployee.Delete response = employee.MapToDeleteResponse();

        _logger.LogInformation($"Employee Id {response.Id}");
        return View(response);
    }

    [HttpDelete(Endpoint.Employees.Delete)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        _logger.LogInformation($"Delete Id: {id}");
        bool IsDeleted = await _services.DeleteByIdAsync(id);

        if (!IsDeleted)
        {
            _logger.LogWarning("The Employe was not deleted");
            return RedirectToAction("Index", "Employees");    
        }

        return RedirectToAction("Index", "Employees");
    }
}
