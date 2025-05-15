using ErpProject.Helpers.Connection;
using ErpProject.Services;
using ErpProject.BackgroundServices;
using ErpProject.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using ErpProject.Controllers;

namespace ErpProject.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<Connection>();

        services.AddScoped<EmployeeServices>();

        services.AddScoped<CredentialsServices>();

        services.AddHostedService<RegistrationCleanUpService>();

        services.AddScoped<AdditionalDetailsServices>();

        services.AddScoped<EmploymentDetailsServices>();

        services.AddScoped<IdentificationsServices>();

        return services;
    }
}
