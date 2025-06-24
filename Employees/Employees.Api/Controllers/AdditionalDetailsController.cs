using Employees.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class AdditionalDetailsController : Controller
{
    private readonly IAdditionalDetailsServices _services;

    public AdditionalDetailsController(IAdditionalDetailsServices services)
    {
        _services = services;
    }
}
