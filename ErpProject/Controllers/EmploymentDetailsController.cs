using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("employmentdetails")]
public class EmploymentDetailsController: Controller
{
    private readonly EmploymentDetailsServices _services;
    
    public EmploymentDetailsController(EmploymentDetailsServices services)
    {
        _services = services;
    }

    [HttpGet("index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        if(id <= 0)
        {
            return NotFound("There is no employee with such Id");
        }

        EmploymentDetails details = await _services.GetEmploymentDetailsAsync(id);

        return PartialView(details);
    }

    [HttpGet("register/{id}")]
    public IActionResult Register(int id)
    {
        if(id <= 0)
        {
            return NotFound("There was a problem");
        }

        EmploymentDetails details = new EmploymentDetails()
        {
            EmployeeId = id
        };

        return PartialView(details);
    }

    [HttpPost("register/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(int id, EmploymentDetails details)
    {
        if(!ModelState.IsValid)
        {
            return PartialView(details);
        }

        if(id <= 0 )
        {
            return NotFound();
        }

        bool result = await _services.AddEmploymentDetailsAsync(id, details);

        if(!result)
        {
            ModelState.AddModelError(string.Empty, "There was a problem with saving your details");
            return RedirectToAction("Details", "Employee", new{id});
        }

        return RedirectToAction("Details", "Employees", new {id});
    }

    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
        if(id <= 0)
        {
            ModelState.AddModelError("id", "There was an error while searching for this employee's details");
            return PartialView("Details", id);
        }

        EmploymentDetails details = new EmploymentDetails()
        {
            EmployeeId = id
        };

        return PartialView(details);
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmploymentDetails details)
    {
        if(id <= 0 || details is null)
        {
            return NotFound("Something went wrong while loading the edit page");
        }

        bool result = await _services.AddEmploymentDetailsAsync(id, details);

        if(!result)
        {
            ModelState.AddModelError(string.Empty, "There was something wrong while saving the employment detials");
            return RedirectToAction("Details", "Employees", new {id});
        }

        return RedirectToAction("Details", "Employees", new {id});
    }
}
