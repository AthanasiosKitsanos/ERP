using Employees.Contracts.AdDetails;
using Employees.Core.Services;
using Employees.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class AdditionalDetailsController : Controller
{
    private readonly IAdditionalDetailsServices _services;

    public AdditionalDetailsController(IAdditionalDetailsServices services)
    {
        _services = services;
    }

    [HttpGet(Endpoint.Views.AdditionalDetailsViews.Get)]
    public IActionResult Get(int id)
    {
        return PartialView(new EmplooyeeId(id));
    }

    [HttpGet(Endpoint.AdditionalDetails.Get)]
    public async Task<IActionResult> GetAdditionalDetails(int id, CancellationToken token)
    {
        ResponseAdditionalDetails.Get details = await _services.GetAsyncById(id, token);

        if (details is null)
        {
            return Json(new { success = false });
        }

        return Json(details);
    }

    [HttpGet(Endpoint.Views.AdditionalDetailsViews.Update)]
    public IActionResult Update()
    {
        return PartialView();
    }

    [HttpPost(Endpoint.AdditionalDetails.Update)]
    public async Task<IActionResult> UpdateAdditionalDetails(int id, RequestAdditionDetails.Update details, CancellationToken token)
    {
        bool IsUpdated = await _services.UpdateAsync(id, details, token);

        if (!IsUpdated)
        {
            return Json(new { success = false });
        }

        return Json(new { success = true });
    }

    [HttpGet(Endpoint.Views.AdditionalDetailsViews.Create)]
    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost(Endpoint.AdditionalDetails.Create)]
    public async Task<IActionResult> CreateAdditionalDetails(int id, RequestAdditionDetails.Create details, CancellationToken token)
    {
        bool IsCreated = await _services.CreateAsync(id, details, token);

        if (!IsCreated)
        {
            return Json(new { success = false });
        }

        return Json(new { success = true });
    }
}
