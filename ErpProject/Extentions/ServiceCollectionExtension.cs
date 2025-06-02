using ErpProject.Helpers.Connection;
using ErpProject.Services;
using ErpProject.BackgroundServices;
using ErpProject.JsonWebToken;
using ErpProject.RefreshTokens;

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

        services.AddSingleton<JWTHeader>();

        services.AddSingleton(sp =>
        {
            IConfiguration config = sp.GetRequiredService<IConfiguration>();
            return new JWTDemoKey(config);
        });

        services.AddSingleton<JWTServices>();

        services.AddScoped<RefreshTokenServices>();

        return services;
    }
}
