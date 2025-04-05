using System;
using ErpProject.Data;
using ErpProject.Helpers.InitializeFolder;
using ErpProject.Services.EmployeeServices;
using ErpProject.Services.RoleServices;
using ErpProject.ContextDb;
using ErpProject.Models.Connection;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<Connection>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<RoleService>();
        return services;
    }
}
