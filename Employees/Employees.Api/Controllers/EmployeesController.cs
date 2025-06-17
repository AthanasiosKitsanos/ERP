using Employees.Core.IServices;
using Employees.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeesServices _services;

    public EmployeesController(IEmployeesServices services)
    {
        _services = services;
    }
}
