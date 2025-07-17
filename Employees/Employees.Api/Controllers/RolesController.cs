using Employees.Contracts.RolesContract;
using Employees.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Api.Controllers;

public class RolesController : Controller
{
    private readonly IRolesServices _services;

    public RolesController(IRolesServices services)
    {
        _services = services;
    }

    [HttpGet(Endpoint.Views.RolesViews.Get)]
    public IActionResult Get()
    {
        return PartialView();
    }

    [HttpGet(Endpoint.Roles.Get)]
    public async Task<IActionResult> GetRole(int id, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        ResponseRoles.Get role = await _services.GetRoleById(id, token);

        return Json(role);
    }

    [HttpGet(Endpoint.Roles.GetAll)]
    public async Task<IActionResult> GetAllRoles(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        List<ResponseRoles.Get> roleList = new List<ResponseRoles.Get>();

        await foreach (ResponseRoles.Get role in _services.GetAllAsync(token))
        {
            roleList.Add(role);
        }

        return Json(roleList);
    }

    [HttpGet(Endpoint.Views.RolesViews.Create)]
    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost(Endpoint.Roles.Create)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoles(int id, RequestRoles.Create create, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool IsCreated = await _services.CreateAsync(id, create, token);

        if (!IsCreated)
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, data = create });
    }

    [HttpGet(Endpoint.Views.RolesViews.Update)]
    public IActionResult Update()
    {
        return PartialView();
    }

    [HttpPost(Endpoint.Roles.Update)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRole(int id, RequestRoles.Update update, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        bool IsUpdated = await _services.UpdateAsync(id, update, token);

        if (!IsUpdated)
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, data = update });
    }
}
