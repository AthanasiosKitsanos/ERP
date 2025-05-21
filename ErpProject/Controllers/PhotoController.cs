using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("photo")]
public class PhotoController : Controller
{
    private readonly PhotoServices _service;

    public PhotoController(PhotoServices service)
    {
        _service = service;
    }

    [HttpGet("index/{id}")]
    public async Task<IActionResult> Index(int id)
    {
        Photo photo = await _service.GetEmployeePhotoAsync(id);

        return PartialView(photo);
    }
}
