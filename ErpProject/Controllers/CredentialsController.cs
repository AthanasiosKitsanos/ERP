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
    public IActionResult Index(int id)
    {
        if (id <= 0)
        {
            ModelState.AddModelError(string.Empty, "Something went wrong.");
            return View();
        }

        return View();
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
            return View(credentials);
        }

        if (!ModelState.IsValid)
        {
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