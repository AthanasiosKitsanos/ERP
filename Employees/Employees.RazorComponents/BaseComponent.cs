using Microsoft.AspNetCore.Components;

namespace Employees.RazorComponents;

public enum ViewMode
{
    View,
    Update,
    Create
}

public class BaseComponent : ComponentBase
{
    [Inject]
    protected NavigationManager Navigate { get; set; } = default!;

    [Inject]
    protected HttpClient HttpClient { get; set; } = default!;

    [Inject]
    protected AntiForgeryServices AntiforgeryService { get; set; } = default!;

    protected MarkupString AntiForgeryMarkup;
    protected ViewMode mode = ViewMode.View;

    protected override async Task OnInitializedAsync()
    {
        AntiForgeryMarkup = await AntiforgeryService.GenerateHiddenMarkupInput();
    }
}
