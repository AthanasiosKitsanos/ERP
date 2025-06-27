using Employees.Contracts.EmployeeContracts;
using Employees.Contracts.EmployeesMapping;
using Employees.Core.IServices;
using Employees.Domain;
using Employees.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Employees.Shared.CustomEndpoints;

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

    [HttpGet(Endpoints.Employees.Index)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet(Endpoints.Employees.GetAllEmployees)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Get All Employees cancelled by client");
            cancellationToken.ThrowIfCancellationRequested();
        }

        List<ResponseEmployee.Get> responseList = new List<ResponseEmployee.Get>();

        await foreach (ResponseEmployee.Get response in _services.GetAllAsync())
        {
            responseList.Add(response);
        }

        if (responseList.Count == 0)
        {
            return View("Error", new ErrorViewModel
            {
                StatusCode = 404,
                Message = "List not found"
            });
        }

        _logger.LogInformation("List of Employees created and was sent as Json.");
        return Json(responseList);
    }

    [HttpGet(Endpoints.Employees.GetAllDetails)]
    public IActionResult Details(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Status code 404, id was not found");

            return PartialView("Error", new ErrorViewModel
            {
                StatusCode = 404,
                Message = "EmployeesController Details: Id was not found"
            });
        }

        EmployeeId newId = new EmployeeId(id);

        return View(newId);
    }

    [HttpGet(Endpoints.Employees.GetMainDetails)]
    public async Task<IActionResult> GetMainDetails(int id, CancellationToken token)
    {
        ResponseEmployee.Get response = await _services.GetByIdAsync(id, token);

        if (response is null)
        {
            _logger.LogInformation($"There was an error while getting the main details");
            return PartialView("Error", new ErrorViewModel
            {
                StatusCode = 404,
                Message = "Employee details not found"
            });
        }

        _logger.LogInformation($"Main details are sent to /employees/{id}/details");

        return PartialView(response);
    }

    [HttpGet(Endpoints.Employees.Create)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost(Endpoints.Employees.Create)]
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

        int employeeId = await _services.CreateAsync(request, cancellationToken);

        if (employeeId <= 0)
        {
            _logger.LogWarning("Employee was not created. Id was 0 or smaller");
            return NotFound();
        }

        _logger.LogInformation($"Employee with Id: {employeeId} is created");

        return RedirectToAction("Create", "Credentials", new { id = employeeId });
    }

    [HttpGet(Endpoints.Employees.Update)]
    public async Task<IActionResult> Update(int id, CancellationToken token)
    {
        ResponseEmployee.Get request = await _services.GetByIdAsync(id, token);

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

    [HttpPut(Endpoints.Employees.Update)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, ResponseEmployee.Update request, CancellationToken token)
    {
        RequestEmployee.Update update = request.MapToUpdateRequest();

        bool IsUpdated = await _services.UpdateAsync(id, update, token);

        if (!IsUpdated)
        {
            _logger.LogWarning($"Employee with Id: {id}, was not updated");
            return RedirectToAction("Details", new { id });
        }

        return RedirectToAction("Details", new { id });
    }

    [HttpGet(Endpoints.Employees.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"url Id {id}");

        ResponseEmployee.Delete response = await _services.GetInfoForDeleteAysnc(id);

        _logger.LogInformation($"Employee Id {response.Id}");
        return View(response);
    }

    [HttpDelete(Endpoints.Employees.Delete)]
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
