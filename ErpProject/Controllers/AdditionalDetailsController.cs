using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("additionaldetails")]
public class AdditionalDetailsController: Controller
{
    private readonly AdditionalDetailsServices _service;

    public AdditionalDetailsController(AdditionalDetailsServices service)
    {
        _service = service;
    }

    [HttpGet("add/{id}")]  
    public IActionResult Add(int id)
    {
        if(id <= 0)
        {
            ModelState.AddModelError(string.Empty, "The was no employee found");
            return RedirectToAction("Index", "Employee");
        }

        ViewData["Id"] = id;

        return View();
    }

    [HttpPost("add/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int id, AdditionalDetails details)
    {
        if(id == 0)
        {
            return NotFound("Employee not Found");
        }

        bool result = await _service.AddAdditionalDetailsAsync(id, details);

        if(!result)
        {
            ModelState.AddModelError(string.Empty, "There was a problem while adding the new details or files.");
            return RedirectToAction("Details", "Employee", new {id});
        }

        return RedirectToAction("Details", "Employee", new {id});
    }

    [HttpGet("update/{id}")]
    public IActionResult Update(int id)
    {
        ViewData["Id"] = id;

        return View();
    }

    [HttpPost("update/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, AdditionalDetails details)
    {
        bool result = await _service.UpdateAdditionalDetailsAsync(id, details);

        if(!result)
        {
            return RedirectToAction("Detials", "Employee", new{id});
        }

        return RedirectToAction("Details", "Employee", new {id});
    }
}