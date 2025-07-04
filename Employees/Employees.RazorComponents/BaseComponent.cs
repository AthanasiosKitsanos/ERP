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
    [Inject] protected NavigationManager Navigate { get; set; } = default!;
    [Inject] protected HttpClient HttpClient { get; set; } = default!;
    protected ViewMode Mode = ViewMode.View;
}
