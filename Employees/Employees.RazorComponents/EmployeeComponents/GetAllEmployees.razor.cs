using Employees.Contracts.EmployeeContracts;
using Microsoft.AspNetCore.Components;
using MvcStyle.Services;

namespace Employees.RazorComponents.EmployeeComponents;

public partial class GetAllEmployees
{
    [Inject] private IControllerServices Controller { get; set; } = default!;
    protected List<ResponseEmployee.Get> employeeList { get; set; } = new List<ResponseEmployee.Get>();

    protected override async Task OnInitializedAsync()
    {
        employeeList = await Controller.HttpGetJsonAsync<List<ResponseEmployee.Get>>("GetAll", "Employees");
    }
    
}
