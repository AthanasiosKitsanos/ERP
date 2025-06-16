using System.Security.Claims;
using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Authorize]
[Route("employmentdetails")]
public class EmploymentDetailsController: Controller
{
    private readonly EmploymentDetailsServices _services;
    
    public EmploymentDetailsController(EmploymentDetailsServices services)
    {
        _services = services;
    }

    [HttpGet("{id}/index")]
    public async Task<IActionResult> Index(int id)
    {
        if (id <= 0)
        {
            return NotFound("There is no employee with such Id");
        }

        if (User.FindFirst(ClaimTypes.Role)?.Value == "Employee" && id != Convert.ToInt32(User.FindFirst("UserId")?.Value))
        {
            EmploymentDetails realDetails = await _services.GetEmploymentDetailsAsync(Convert.ToInt32(User.FindFirst("UserId")?.Value));

            ViewBag.Breach = "Nice Try";
            return PartialView(realDetails);
        }

        EmploymentDetails details = await _services.GetEmploymentDetailsAsync(id);

        return PartialView(details);
    }

    [HttpGet("{id}/register")]
    [Authorize(Roles = "Admin, Manager")]
    public IActionResult Register(int id)
    {
        if (id <= 0)
        {
            return NotFound("There was a problem");
        }

        EmploymentDetails details = new EmploymentDetails()
        {
            EmployeeId = id
        };

        return PartialView(details);
    }

    [HttpPost("{id}/register")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Register(int id, EmploymentDetails details)
    {
        if (!ModelState.IsValid)
        {
            return PartialView(details);
        }

        if (id <= 0)
        {
            return NotFound();
        }

        bool result = await _services.AddEmploymentDetailsAsync(id, details);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "There was a problem with saving your details");
            return RedirectToAction("Details", "Employees", new { id });
        }

        return RedirectToAction("Details", "Employees", new { id });
    }

    [HttpGet("{id}/edit")]
    [Authorize(Roles = "Admin, Manager")]
    public IActionResult Edit(int id)
    {
        if (id <= 0)
        {
            ModelState.AddModelError("id", "There was an error while searching for this employee's details");
            return PartialView("Index", "Home");
        }

        EmploymentDetails details = new EmploymentDetails()
        {
            EmployeeId = id
        };

        return PartialView(details);
    }

    [HttpPut("{id}/edit")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Edit(int id, EmploymentDetails details)
    {
        if (id <= 0 || details is null)
        {
            return NotFound("Something went wrong while loading the edit page");
        }

        bool result = await _services.UpdateEmploymentDetailsAsync(id, details);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "There was something wrong while saving the employment detials");
            return RedirectToAction("Details", "Employees", new { id });
        }

        return RedirectToAction("Details", "Employees", new { id });
    }
}
