using Employees.Contracts.CredentialsContract;
using Employees.Contracts.CredentialsMapping;
using Employees.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Employees.Core.Services;

namespace Employees.Api.Controllers;

public class CredentialsController : Controller
{
    private readonly ICredentialsServices _services;
    private readonly ILogger<CredentialsController> _logger;

    public CredentialsController(ICredentialsServices services, ILogger<CredentialsController> logger)
    {
        _services = services;
        _logger = logger;
    }

    [HttpGet(Endpoint.Views.CredentialsViews.Create)]
    public IActionResult Create(int id)
    {

        RequestCredentials.Create request = new RequestCredentials.Create { EmployeeId = id };

        return View(request);
    }

    [HttpPost(Endpoint.Views.CredentialsViews.Create)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int id, RequestCredentials.Create request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        bool usernameExists = await _services.UsernameExistsAsync(request.Username, cancellationToken);

        if (usernameExists)
        {
            ModelState.AddModelError(nameof(request.Username), "Username already exists");
            return View(request);
        }

        Credentials credentials = request.MapToCreateRequest(id);

        bool IsCreated = await _services.CreateAsync(credentials, cancellationToken);

        if (!IsCreated)
        {
            ModelState.AddModelError(string.Empty, "There was something wrong while adding the credentials");
            _logger.LogWarning("Credentials were not created");

            return View(request);
        }

        _logger.LogInformation("Credentials were created");

        return RedirectToAction("Index", "Employees");
    }
}