using Employees.Contracts.AdditionalDetailsContract;
using Employees.Contracts.AdditionalDetailsMapping;
using Employees.Core.IServices;
using Employees.Domain;
using Employees.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class AdditionalDetailsController : Controller
{
    private readonly IAdditionalDetailsServices _services;
    private readonly ILogger<AdditionalDetailsController> _logger;

    public AdditionalDetailsController(IAdditionalDetailsServices services, ILogger<AdditionalDetailsController> logger)
    {
        _services = services;
        _logger = logger;
    }

    [HttpGet(Endpoint.AdditionalDetails.Get)]
    public async Task<IActionResult> Get(int id, CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            _logger.LogWarning("Request cancelled by user");
            token.ThrowIfCancellationRequested();
        }

        ResponseAdditionalDetails.Get details = await _services.GetAsync(id, token);

        if (details is null)
        {
            _logger.LogWarning($"{nameof(details)} were not found");

            return PartialView("Error", new ErrorViewModel
            {
                StatusCode = 404,
                Message = "Additional Details not found"
            });
        }

        _logger.LogInformation($"{nameof(details)} are sent to employees/{id}/details from Url employees/{id}/additionaldetails");

        return PartialView(details);
    }

    [HttpGet(Endpoint.AdditionalDetails.Create)]
    public IActionResult Create(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("There was no Id found");
            return PartialView("Error", new ErrorViewModel
            {
                StatusCode = 404,
                Message = "No Id was found"
            });
        }

        _logger.LogInformation("Additional Details Create page loaded");

        return PartialView(new RequestAdditionalDetails.Create { EmployeeId = id });
    }

    [HttpPost(Endpoint.AdditionalDetails.Create)]
    public async Task<IActionResult> Create(int id, RequestAdditionalDetails.Create details, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Employees", new { id });
        }
        
        _logger.LogWarning($"{nameof(details.EmployeeId)} = {details.EmployeeId}");        

        bool IsCreated = await _services.CreateAsync(id, details, token);

        if (!IsCreated)
        {
            _logger.LogInformation($"{details.EmployeeId}");
            _logger.LogWarning("The details were not added");
            return RedirectToAction("Details", "Employees", new { id = details.EmployeeId });
        }

        return RedirectToAction("Details", "Employees", new { id });
    }

    [HttpGet(Endpoint.AdditionalDetails.Update)]
    public async Task<IActionResult> Update(int id, CancellationToken token)
    {
        ResponseAdditionalDetails.Get details = await _services.GetAsync(id, token);

        RequestAdditionalDetails.Update update = details.MapToUpdateRequest(id);

        return PartialView(update);
    }

    [HttpPut(Endpoint.AdditionalDetails.Update)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, RequestAdditionalDetails.Update details, CancellationToken token)
    {
        bool IsUpdated = await _services.UpdateAsync(id, details, token);

        if (!IsUpdated)
        {
            _logger.LogWarning("There was an error while updating the additional details");
            return PartialView("Error", new ErrorViewModel
            {
                StatusCode = 400,
                Message = "There was an error while updating the additional details"
            });
        }

        _logger.LogInformation("Additional details updated");

        return RedirectToAction("Details", "Employees", new { id });
    }
}
