using System;
using ErpProject.Models.DTOModels;
using ErpProject.Services;
using ErpProject.Services.EmployeeCredentialsServices;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("employeecredentials")]
public class EmployeeCredentialsController: Controller
{
    private readonly RegistrationService _service;

    private readonly EmployeeCredentialsService _ecService;

    public EmployeeCredentialsController(RegistrationService service, EmployeeCredentialsService ecService)
    {
        _service = service;
        _ecService = ecService;
    }

    [HttpGet("add")]
    public async Task<IActionResult> Add(ViewModelDTO model)
    {
        if(model is null)
        {
            return NotFound();
        }

        model.AccountStatus.StatusList = await _ecService.GetAccountStatusAsync();

        return View(model);
    }

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCredentials(ViewModelDTO model)
    {
        if(model is null)
        {
            return NotFound();
        }

        bool result =  await _service.RegistrationCompleteAsync(model);

        if(!result)
        {
            return RedirectToAction("Register", "Employee");
        }

        return RedirectToAction("Index", "Employee");
    }
}