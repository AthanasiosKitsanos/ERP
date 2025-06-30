using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace Employees.RazorComponents;

public class AntiForgeryServices
{
    private readonly IAntiforgery _antiforgery;
    private readonly IHttpContextAccessor _httpAccessor;

    public AntiForgeryServices(IAntiforgery antiforgery, IHttpContextAccessor httpAccessor)
    {
        _antiforgery = antiforgery;
        _httpAccessor = httpAccessor;
    }

    public Task<MarkupString> GenerateHiddenMarkupInput()
    {
        HttpContext context = _httpAccessor.HttpContext;

        if (context is null)
        {
            return Task.FromResult(new MarkupString(string.Empty));
        }

        AntiforgeryTokenSet token = _antiforgery.GetAndStoreTokens(context);

        return Task.FromResult(new MarkupString($"<input type=\"hidden\" name=\"{token.FormFieldName}\" value=\"{token.RequestToken}\" />"));
    }
}
