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

    [HttpGet("getdetails/{id}")]
    public async Task<IActionResult> GetDetails(int id)
    {
        if(id <= 0)
        {
            return NotFound("There is no employee with such Id");
        }

        EmploymentDetails details = await _services.GetEmploymentDetailsAsync(id);

        return Json(details);
    }
}
