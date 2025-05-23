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

    [HttpGet("index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        if(id <= 0)
        {
            ModelState.AddModelError(string.Empty, "There was a problem loading the page");
            return View();
        }

        AdditionalDetails details = await _service.GetAdditionalDetailsAsync(id);

        if(details.EmployeeId <= 0)
        {
            details.EmployeeId = id;    
        }
        
        return PartialView(details);
    }

    [HttpGet("register/{id}")]  
    public IActionResult Register(int id)
    {
        if(id <= 0)
        {
            ModelState.AddModelError(string.Empty, "The was no employee found");
            return RedirectToAction("Details", "Employee");
        }

        AdditionalDetails details = new AdditionalDetails()
        {
            EmployeeId = id
        };

        return PartialView(details);
    }

    [HttpPost("register/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(int id, AdditionalDetails details)
    {
        if(!ModelState.IsValid)
        {
            return PartialView(details);
        }

        if(id <= 0)
        {
            return NotFound($"Employee not Found {id}");
        }

        bool result = await _service.AddAdditionalDetailsAsync(id, details);

        if(!result)
        {
            ModelState.AddModelError(string.Empty, "There was a problem while adding the new details or files.");
            return RedirectToAction("Details", "Employee", new {id});
        }

        return RedirectToAction("Details", "Employee", new {id});
    }

    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
        if(id <= 0)
        {
            ModelState.AddModelError(string.Empty, "The was no employee found");
            return RedirectToAction("Details", "Employee");
        }
        
        AdditionalDetails details = new AdditionalDetails()
        {
            EmployeeId = id
        };

        return PartialView(details);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AdditionalDetails details)
    {
        if(id <= 0)
        {
            return NotFound("No Employee is found to edit the details");
        }

        bool result = await _service.UpdateAdditionalDetailsAsync(id, details);

        if(!result)
        {
            return RedirectToAction("Detials", "Employee", new{id});
        }

        return RedirectToAction("Details", "Employee", new {id});
    }
}