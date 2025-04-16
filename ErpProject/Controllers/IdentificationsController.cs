using System.Threading.Tasks;
using ErpProject.Helpers;
using ErpProject.Models.DTOModels;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("identifications")]
public class IdentificationsController: Controller
{
    [HttpGet("add")]
    public IActionResult Add(ViewModelDTO model)
    {
        if(model is null)
        {
            return NotFound(model);
        }

        return View(model);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddDetails(ViewModelDTO model)
    {
        if(model is null)
        {
            return RedirectToAction("Add", model);
        }

        bool result = await TinValidation.IsValidTin(model.Identifications.TIN);

        if(!result)
        {
            ViewBag.Add("The TIN number is invalid");
            return View(model);
        }

        return RedirectToAction("Add", "Roles", model);
    }
}