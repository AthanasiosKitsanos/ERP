using ErpProject.Helpers.Connection;
using ErpProject.Services;
using ErpProject.BackgroundServices;

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

        services.AddScoped<PhotoServices>();

        services.AddScoped<RolesServices>();

        services.AddScoped<LogInServices>();

        return services;
    }
}
