using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.ViewComponents;

[Route("additionaldetailsvc")]
public class AdditionalDetailsViewComponent: ViewComponent
{
    private readonly AdditionalDetailsServices _service;

    public AdditionalDetailsViewComponent(AdditionalDetailsServices service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        ViewData["Id"] = id;

        AdditionalDetails details = await _service.GetAdditionalDetailsAsync(id);
         
        if(details is null)
        {
            return View();
        }

        return View(details);
    } 
}
