using Employees.Contracts.EmployeeContracts;
using Microsoft.AspNetCore.Components;
using MvcStyle.Services;

namespace Employees.RazorComponents.EmployeeComponents;

public partial class GetMainDetails
{
    [Inject] private IControllerServices Controller { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }

    protected ResponseEmployee.Get? employee;
    protected ResponseEmployee.Update update = new ResponseEmployee.Update();

    protected override async Task OnInitializedAsync()
    {
        employee = await Controller.HttpGetJsonAsync<ResponseEmployee.Get>("GetMainDetails", "Employees", Id);
    }

    protected void UpdateForm()
    {
        Mode = ViewMode.Update;
    }

    protected void CancelForm()
    {
        Mode = ViewMode.View;
    }

    protected async Task OnSubmit()
    {
        employee = await Controller.HttpGetJsonAsync<ResponseEmployee.Get>("GetMainDetails", "Employees", Id);

        Mode = ViewMode.View;
    }
}