using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.ViewComponents;

public class EmploymentDetailsViewComponent: ViewComponent
{
    private readonly EmploymentDetailsServices _services;

    public EmploymentDetailsViewComponent(EmploymentDetailsServices services)
    {
        _services = services;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        EmploymentDetails details = new EmploymentDetails();
        return View(details);
    }
}
