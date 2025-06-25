using Employees.Contracts.AdditionalDetailsContract;
using Employees.Core.IServices;
using Employees.Domain;
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

        _logger.LogInformation($"{nameof(details)} are sent to employees/{id}/details\nFrom Url employees/{id}/additionaldetails");

        return PartialView(details);
    }

    
}
