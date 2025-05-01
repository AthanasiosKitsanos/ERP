using ErpProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.ViewComponents;

public class AdditionalDetailsVC: ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(AdditionalDetails details)
    {
        if(details is null)
        {
            return View();
        }

        return View(details);
    }
}
