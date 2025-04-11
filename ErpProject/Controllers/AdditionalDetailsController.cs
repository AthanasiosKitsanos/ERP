using System;
using ErpProject.Models.DTOModels;
using ErpProject.Services.EmployeeServicesFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ErpProject.Controllers;

[Route("additionaldetails")]
public class AdditionalDetailsController: Controller
{
    private readonly AdditionalDetailsService _service;

    public AdditionalDetailsController(AdditionalDetailsService service)
    {
        _service = service;
    }

    [HttpGet("add")]
    public IActionResult Add(ViewModelDTO model)
    {
        if(model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("add")]
    public IActionResult AddDetails(ViewModelDTO model)
    {
        if(model is null)
        {
            return RedirectToAction("Add");
        }

        return RedirectToAction("Add", "EmploymentDetails", model);
    }
}
