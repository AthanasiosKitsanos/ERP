using Employees.Contracts.EmpDe;
using Employees.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class EmploymentDetailsController : Controller
{
    private readonly IEmploymentDetailsServices _services;

    public EmploymentDetailsController(IEmploymentDetailsServices services)
    {
        _services = services;
    }

    [HttpGet(Endpoint.Views.EmploymentDetailsViews.Get)]
    public IActionResult Get()
    {
        return PartialView();
    }

    [HttpGet(Endpoint.EmploymentDetails.Get)]
    public async Task<IActionResult> GetEmploymentDetails(int id, CancellationToken token)
    {
        ResponseEmploymentDetails.Get details = await _services.GetByIdAsync(id, token);

        return Json(details);
    }

    [HttpGet(Endpoint.Views.EmploymentDetailsViews.Create)]
    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost(Endpoint.EmploymentDetails.Create)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAdditionalDetails(int id, RequestEmploymentDetails.Create request, CancellationToken token)
    {
        bool IsCreate = await _services.CreateAsync(id, request, token);

        if (!IsCreate)
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, data = request });
    }

    [HttpGet(Endpoint.Views.EmploymentDetailsViews.Update)]
    public IActionResult Update()
    {
        return PartialView();
    }

    [HttpPost(Endpoint.EmploymentDetails.Update)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEmploymentDetails(int id, RequestEmploymentDetails.Update update, CancellationToken token)
    {
        bool IsUpdated = await _services.UpdateAsync(id, update, token);

        if (!IsUpdated)
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, data = update });
    }
}
