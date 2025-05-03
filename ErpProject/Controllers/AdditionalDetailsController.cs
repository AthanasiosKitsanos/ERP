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
    public IActionResult SwitchToEditMode(int id)
    {
        return ViewComponent("AdditionalDetails", new{id, mode = "edit"});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAdditionalDetails(int id, AdditionalDetails details)
    {
        if(!ModelState.IsValid)
        {
            return ViewComponent("AdditionalDetails", new {id});
        }

        bool result = await _service.SaveAdditionalDetailsAsync(id, details);

        if(id == 0)
        {
            return NotFound("Employee not Found");
        }

        return ViewComponent("AdditionalDetails", new {id});
    }
}