using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Authorize(Roles = "Admin, Manager")]
[Route("credentials")]
public class CredentialsController: Controller
{
    private readonly CredentialsServices _service;

    public CredentialsController(CredentialsServices service)
    {
        _service = service;
    }

    [HttpGet("index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        if (id <= 0)
        {
            ModelState.AddModelError(string.Empty, "Something went wrong.");
            return View();
        }

        Credentials credentials = new Credentials();

        credentials.StatusNameList = await _service.GetAccountStatusListAsync();

        if (credentials.StatusNameList is null || credentials.StatusNameList.Count == 0)
        {
            ModelState.AddModelError("Status List", "Something went wrong while loading the status list");
            return View();
        }

        return View(credentials);
    }

    [HttpPost("index/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(int id, Credentials credentials)
    {
        if (credentials is null)
        {
            ModelState.AddModelError(string.Empty, "There was something wring while adding the credentials");
            return View(new Credentials());
        }

        if (await _service.UsernameExistsAsync(credentials.Username))
        {
            ModelState.AddModelError("Username", "Username already exists!");
            credentials.StatusNameList = await _service.GetAccountStatusListAsync();
            return View(credentials);
        }

        if (!ModelState.IsValid)
        {
            credentials.StatusNameList = await _service.GetAccountStatusListAsync();
            return View(credentials);
        }

        if (!await _service.CreateCredentialsAsync(id, credentials))
        {
            ModelState.AddModelError(string.Empty, "There was something wring while creating the credentials");
            return View(credentials);
        }

        return RedirectToAction("Index", "LogIn");
    }
}