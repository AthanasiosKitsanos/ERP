using System;
using ErpProject.Services.EmployeeServices;
using ErpProject.Services.RoleServices;
using ErpProject.ContextDb;
using ErpProject.Helpers.Connection;
using ErpProject.Models.AdditionalDetailsModel;
using ErpProject.Services.EmployeeServicesFolder;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<Connection>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<RoleService>();
        services.AddScoped<AdditionalDetailsService>();
        return services;
    }
}
