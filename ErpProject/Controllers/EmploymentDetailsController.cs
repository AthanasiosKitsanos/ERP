using ErpProject.Models.DTOModels;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("employmentDetails")]
public class EmploymentDetailsController: Controller
{
    private readonly FileUpload _uploadService;

    public EmploymentDetailsController(FileUpload uploadService)
    {
        _uploadService = uploadService;
    }

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
            return RedirectToAction("Add");
        }

        return RedirectToAction("Add", "Identifications", model);
    }
}
