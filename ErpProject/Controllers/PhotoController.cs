using System.Security.Claims;
using ErpProject.Models;
using ErpProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Authorize]
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
        if (id <= 0)
        {
            return NotFound("Picture not found");
        }

        if (User.FindFirst(ClaimTypes.Role)?.Value == "Emplpyee" && id != Convert.ToInt32(User.FindFirst("UserId")?.Value))
        {
            Photo realPhoto = await _service.GetEmployeePhotoAsync(Convert.ToInt32(User.FindFirst("UserId")?.Value));
            return PartialView(realPhoto);
        }

        Photo photo = await _service.GetEmployeePhotoAsync(id);

        return PartialView(photo);
    }
}
