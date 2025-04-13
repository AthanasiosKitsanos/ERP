using System;
using ErpProject.Services.EmployeeServices;
using ErpProject.Services.RoleServices;
using ErpProject.ContextDb;
using ErpProject.Helpers.Connection;
using ErpProject.Models.AdditionalDetailsModel;
using ErpProject.Services.EmployeeServicesFolder;
using ErpProject.Services;
using ErpProject.Helpers;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<Connection>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<RoleService>();
        services.AddScoped<AdditionalDetailsService>();
        services.AddScoped<EmploymentDetailsService>();
        services.AddScoped<RegistrationService>();
        services.AddScoped<FileManagement>();
        return services;
    }
}
