using Employees.Shared.CustomEndpoints;
using System.Net.Http.Json;
using Employees.Contracts.EmployeeContracts;

namespace Employees.RazorComponents.EmployeeComponents;

public partial class GetAllEmployees : BaseComponent
{
    protected List<ResponseEmployee.Get> employeeList { get; set; } = new List<ResponseEmployee.Get>();

    protected override async Task OnInitializedAsync()
    {
        employeeList = await HttpClient.GetFromJsonAsync<List<ResponseEmployee.Get>>($"{Navigate.BaseUri.TrimEnd('/')}{Endpoints.Employees.GetAllEmployees}") ?? null!;
    }
    
}
