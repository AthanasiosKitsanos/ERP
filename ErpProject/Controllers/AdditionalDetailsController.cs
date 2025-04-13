using ErpProject.Models.DTOModels;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("additionaldetails")]
public class AdditionalDetailsController: Controller
{
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
    [ValidateAntiForgeryToken]
    public IActionResult AddDetails(ViewModelDTO model)
    {
        if(model is null)
        {
            return RedirectToAction("Add");
        }

        return RedirectToAction("Add", "EmploymentDetails", model);
    }
}
