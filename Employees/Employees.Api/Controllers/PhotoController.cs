using Employees.Domain;
using Employees.Contracts.File;
using Employees.Core.IServices;
using Employees.Shared.CustomEndpoints;

using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class FileController : Controller
{
    private readonly IFileServices _services;
    private readonly ILogger<FileController> _logger;

    public FileController(IFileServices services, ILogger<FileController> logger)
    {
        _services = services;
        _logger = logger;
    }



    [HttpGet(Endpoints.Files.GetPhoto)]
    public async Task<IActionResult> GetPhoto(int id, CancellationToken token)
    {
        ResponseFile.GetPhoto photo = await _services.GetPhotoAsync(id, token);

        if (photo is null)
        {
            _logger.LogInformation($"Employee {id} photograph was not found");

            return PartialView("Error", new ErrorViewModel
            {
                StatusCode = 404,
                Message = "Photo not found"
            });
        }

        _logger.LogInformation($"Photo was sent to /employees/{id}/details from /employees/{id}/files/photograph");

        return File(photo.Photograph, photo.Mime);
    }
}
