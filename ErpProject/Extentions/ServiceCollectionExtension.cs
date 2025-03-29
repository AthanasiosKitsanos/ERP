using System;
using ErpProject.Data;
using ErpProject.Helpers.InitializeFolder;
using ErpProject.Interfaces.EmployeeInterfaces;
using ErpProject.Services.EmployeeServices;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmploymentDetailsService, EmploymentDetailsService>();
        // services.AddScoped<CreateFirstElements>();
        // services.AddScoped<SeedData>();
        return services;
    }
}
