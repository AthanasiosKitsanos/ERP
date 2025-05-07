using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("employmentdetails")]
public class EmploymentDetailsController: Controller
{
    private readonly EmploymentDetailsServices _services;
    
    public EmploymentDetailsController(EmploymentDetailsServices services)
    {
        _services = services;
    }
}
