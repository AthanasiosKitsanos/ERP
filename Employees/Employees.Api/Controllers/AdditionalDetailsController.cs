using Employees.Contracts.AdditionalDetailsContract;
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

        return PartialView(new RequestAdditionalDetails.Create { Id = id });
    }

    [HttpPost(Endpoint.AdditionalDetails.Create)]
    public async Task<IActionResult> Create(RequestAdditionalDetails.Create details, CancellationToken token)
    {
        bool IsCreated = await _services.CreateAsync(details, token);

        return RedirectToAction("Details", "Employees", new { details.Id });
    }
}
