using ErpProject.JsonWebToken;
using Microsoft.AspNetCore.Authentication;

namespace ErpProject.Extentions;

public static class DISetupExtention
{
    public static IServiceCollection AddDISetup(this IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddAntiforgery();

        services.AddAuthentication("ErpJwt").AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("ErpJwt", null);

        services.AddAuthorization();

        return services;
    }
}
