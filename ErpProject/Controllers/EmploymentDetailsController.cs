using System;
using ErpProject.Interfaces.EmployeeInterfaces;
using Microsoft.AspNetCore.Mvc;
using ErpProject.Services.EmployeeServices;
using ErpProject.Models.DTOModels.EmployeeDTO;


namespace ErpProject.Controllers;

public class EmploymentDetailsController: Controller
{
    private readonly IEmploymentDetailsService _edService;

    public EmploymentDetailsController(IEmploymentDetailsService edService)
    {
        _edService = edService;
    }

    [HttpPost("employmentdetailsform/{id}")]
    public async Task<IActionResult> EmploymentDetailsForm(EmploymentDetailsDTO dto, int id)
    {
        var result = await _edService.AddEmploymentDetailsAsync(dto, id);

        if(!result)
        {
            return View("No details were added.");
        }

        return View();
    }
}
