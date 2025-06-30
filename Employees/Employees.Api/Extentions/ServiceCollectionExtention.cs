namespace Employees.Api.Extentions;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddRazorComponents().AddInteractiveServerComponents();
        
        //services.AddServerSideBlazor();

        services.AddHttpClient();
        
        services.AddAntiforgery();

        services.AddHttpContextAccessor();

        //services.AddAuthentication("ErpJwt").AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("ErpJwt", options => {});

        services.AddAuthorization();
        
        
        return services;
    }
}