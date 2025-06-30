using System.Net.Http.Json;
using Employees.Contracts.EmployeeContracts;
using Employees.Shared.CustomEndpoints;
using Microsoft.AspNetCore.Components;

namespace Employees.RazorComponents.EmployeeComponents;

public class MainDetailsComponent : BaseComponent
{
    [Parameter]
    public int Id { get; set; }

    protected ResponseEmployee.Get? employee;

    protected RequestEmployee.Update update = new RequestEmployee.Update();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        employee = await HttpClient.GetFromJsonAsync<ResponseEmployee.Get>($"{Navigate.BaseUri.TrimEnd('/')}{Endpoints.Employees.GetMainDetails.Replace("{id}", Id.ToString())}") ?? null!;
    }

    protected void UpdateForm()
    {
        Mode = ViewMode.Update;
    }

    protected void CancelForm()
    {
        Mode = ViewMode.View;
    }
}

// Μην ξεχάσεις στο OnInitializedAsync
//πρώτα να καλεσεις το base.OnInitializedAsync για να βαλει το AntiForgeryToken