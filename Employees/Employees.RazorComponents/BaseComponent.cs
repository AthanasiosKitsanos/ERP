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
    protected ViewMode Mode = ViewMode.View;
}
