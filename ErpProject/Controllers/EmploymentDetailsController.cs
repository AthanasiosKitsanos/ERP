using ErpProject.Models.DTOModels;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("employmentDetails")]
public class EmploymentDetailsController: Controller
{
    [HttpGet("add")]
    public IActionResult Add(ViewModelDTO model)
    {
        if(model is null)
        {
            return NotFound("No information was found");
        }

        return View(model);
    }

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public IActionResult AddDetails(ViewModelDTO model)
    {
        if(model is null)
        {
            return RedirectToAction("Add", model);
        }

        return RedirectToAction("Add", "Identifications", model);
    }
}
