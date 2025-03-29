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

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var employmentDetails = await _edService.GetEmploymentDetailsAsync(id);

        if(employmentDetails is null)
        {
            return NotFound();
        }

        return View(employmentDetails);
    }

    [HttpPost("create/{id}")]
    public async Task<IActionResult> Create(EmploymentDetailsDTO dto, int id)
    {
        var result = await _edService.AddEmploymentDetailsAsync(dto, id);

        if(!result)
        {
            return View("No details were added.");
        }

        return View();
    }
}
