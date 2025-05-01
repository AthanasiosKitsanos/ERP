using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ErpProject.Controllers;

public class AdditionalDetailsController: Controller
{
    private readonly AdditionalDetailsServices _service;

    public AdditionalDetailsController(AdditionalDetailsServices service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAdditionalDetails(int id, AdditionalDetails details)
    {
        if(id == 0)
        {
            return ViewComponent("AdditionalDetails");
        }

        details = await _service.GetAdditionalDetailsAsync(id);

        if(details is null)
        {
            ModelState.AddModelError(string.Empty, "No additional details were found");
            return ViewComponent("AdditionalDetails");
        }

        return ViewComponent("AdditionalDetails", details);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAdditionalDetails(int id, AdditionalDetails details)
    {
        if(!ModelState.IsValid)
        {
            return ViewComponent("AdditionalDetails", new {id});
        }

        //bool result = await _service.

        if(id == 0)
        {
            return NotFound("Employee not Found");
        }

        return RedirectToAction("Details", "Employee");
    }
}