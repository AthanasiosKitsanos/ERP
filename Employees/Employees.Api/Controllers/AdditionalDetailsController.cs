using Employees.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class AdditionalDetailsController : Controller
{
    private readonly IAdditionalDetailsServices _services;

    public AdditionalDetailsController(IAdditionalDetailsServices services)
    {
        _services = services;
    }
    
    //TODO: Create Endpoints and the rest of the controller
}
