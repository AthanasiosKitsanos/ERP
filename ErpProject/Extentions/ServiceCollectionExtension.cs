using ErpProject.Services.EmployeeServices;
using ErpProject.Services.RoleServices;
using ErpProject.Helpers.Connection;
using ErpProject.Services;
using ErpProject.Helpers;
using ErpProject.Models.RegistrationModel;
using ErpProject.Services.AdditionalDetailsServices;
using ErpProject.Services.EmploymentDetailsServices;
using ErpProject.Services.IdentificationServices;
using ErpProject.Services.EmployeeCredentialsServices;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<Connection>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<AdditionalDetailsService>();
        services.AddScoped<EmploymentDetailsService>();
        services.AddScoped<RegistrationService>();
        services.AddScoped<FileManagement>();
        services.AddScoped<IdentificationsService>();
        services.AddScoped<RegistrationModel>();
        services.AddScoped<RoleService>();
        services.AddScoped<EmployeeCredentialsService>();
        return services;
    }
}
