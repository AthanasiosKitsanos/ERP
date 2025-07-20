using Employees.Contracts.EmployeeContracts;
using Employees.Core.Services;
using Employees.Domain;
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

    [HttpGet(Endpoint.Views.EmployeeViews.Index)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet(Endpoint.Employees.GetAllEmployees)]
    public async Task<IActionResult> GetAllEmployees(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        List<ResponseEmployee.Get> responseList = new List<ResponseEmployee.Get>();


        await foreach (ResponseEmployee.Get response in _services.GetAllAsync())
        {
            responseList.Add(response);
        }

        return Json(responseList);
    }

    [HttpGet(Endpoint.Views.EmployeeViews.Details)]
    public IActionResult Details(int id)
    {
        return View(new EmployeeId(id));
    }

    [HttpGet(Endpoint.Employees.Get)]
    public async Task<IActionResult> GetMainDetails(int id, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        ResponseEmployee.Get response = await _services.GetByIdAsync(id, token);

        if (response is null)
        {
            return Json(new
            {
                success = false,
                error = "Employee details not found"
            });
        }

        return Json(response);
    }

    [HttpGet(Endpoint.Views.EmployeeViews.Create)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost(Endpoint.Views.EmployeeViews.Create)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RequestEmployee.Create request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Model State not valid, returning to Create Page again.");
            return View(request);
        }

        bool emailExists = await _services.EmailExistsAsync(request.Email);

        if (emailExists)
        {
            ModelState.AddModelError(nameof(request.Email), "Email already exists");
            return View(request);
        }

        int employeeId = await _services.CreateAsync(request, cancellationToken);

        if (employeeId <= 0)
        {
            _logger.LogWarning("Employee was not created. Id was 0 or smaller");
            return NotFound();
        }

        return RedirectToAction("Create", "Credentials", new { id = employeeId });
    }

    [HttpGet(Endpoint.Views.EmployeeViews.GetMainDetails)]
    public IActionResult GetMainDetails(int id)
    {
        return PartialView();
    }

    [HttpGet(Endpoint.Views.EmployeeViews.Update)]
    public IActionResult Update()
    {
        return PartialView();
    }

    [HttpPost(Endpoint.Employees.Update)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, RequestEmployee.Update request, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        
        bool IsUpdated = await _services.UpdateAsync(id, request, token);

        if (!IsUpdated)
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, data = request });
    }

    [HttpGet(Endpoint.Views.EmployeeViews.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        ResponseEmployee.Delete response = await _services.GetInfoForDeleteAysnc(id);
        return View(response);
    }

    [HttpPost(Endpoint.Views.EmployeeViews.Delete)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        bool IsDeleted = await _services.DeleteByIdAsync(id);

        if (!IsDeleted)
        {
            return RedirectToAction("Index", "Employees");    
        }

        return RedirectToAction("Index", "Employees");
    }
}