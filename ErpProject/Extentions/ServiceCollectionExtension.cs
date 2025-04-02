using System;
using ErpProject.Data;
using ErpProject.Helpers.InitializeFolder;
using ErpProject.Services.EmployeeServices;
using ErpProject.Services.RoleServices;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<EmployeeService>();
        services.AddScoped<RoleService>();
        //services.AddScoped<EmploymentDetailsService>();
        // services.AddScoped<CreateFirstElements>();
        // services.AddScoped<SeedData>();
        return services;
    }
}
