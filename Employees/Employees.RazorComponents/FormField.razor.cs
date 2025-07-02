using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Employees.RazorComponents.Form;

public class FormFieldComponent : BaseComponent
{
    [Inject] private IHttpContextAccessor Accessor { get; set; } = default!;
    [Inject] private IUrlHelperFactory UrlHelper { get; set; } = default!;
    [Parameter] public string? action { get; set; }
    [Parameter] public string? controller { get; set; }
    [Parameter] public int routeId { get; set; }
    [Parameter] public string? method { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected Dictionary<string, object>? RouteValue { get; set; }
    protected string? ActionUrl { get; set; }

    protected override async Task OnInitializedAsync()
    {
        AntiForgeryMarkup = await AntiforgeryService.GenerateHiddenMarkupInput();

        string id = routeId.ToString();

        if (!string.IsNullOrEmpty(id))
        {
            RouteValue = new Dictionary<string, object>
            {
                ["id"] = id
            };
        }

        HttpContext httpContext = Accessor.HttpContext;

        if (httpContext is not null)
        {
            var actionContext = new ActionContext
            (
                httpContext,
                httpContext.GetRouteData() ?? new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
            );

            IUrlHelper url = UrlHelper.GetUrlHelper(actionContext);
            ActionUrl = url.Action(action, controller, RouteValue);
        }
    }
}
