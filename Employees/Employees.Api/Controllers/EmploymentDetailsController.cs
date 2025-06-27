using Employees.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class EmploymentDetailsController : Controller
{
    private readonly IEmploymentDetailsServices _services;
    private readonly ILogger<EmploymentDetailsController> _logger;

    public EmploymentDetailsController(IEmploymentDetailsServices services, ILogger<EmploymentDetailsController> logger)
    {
        _services = services;
        _logger = logger;
    }
}
