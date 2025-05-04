using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.ViewComponents;

public class AdditionalDetailsViewComponent: ViewComponent
{
    private readonly AdditionalDetailsServices _service;

    public AdditionalDetailsViewComponent(AdditionalDetailsServices service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        AdditionalDetails details = await _service.GetAdditionalDetailsAsync(id);
        
        if(details is null)
        {
            return View();
        }

        ViewData["Id"] = id;

        return View(details);
    }
}
