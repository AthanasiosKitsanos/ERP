using ErpProject.JsonWebToken;
using ErpProject.Models;
using ErpProject.Services;

namespace ErpProject.RefreshTokens;

public class RefreshTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;


    public RefreshTokenMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path.Value?.ToLower() ?? "";

        if (path.StartsWith("/login") || path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/images"))
        {
            await _next(context);
            return;
        }

        using IServiceScope scoped = _scopeFactory.CreateScope();

        RefreshTokenServices _tokenServices = scoped.ServiceProvider.GetRequiredService<RefreshTokenServices>();
        JWTServices _jwtServices = scoped.ServiceProvider.GetRequiredService<JWTServices>();
        LogInServices _logInServices = scoped.ServiceProvider.GetRequiredService<LogInServices>();

        
        if (!context.Request.Cookies.TryGetValue("refreshtoken", out string? token) || string.IsNullOrEmpty(token))
        {
            context.Response.Cookies.Delete("jwt");
            context.Response.Cookies.Delete("refreshtoken");

            context.Response.Redirect("/LogIn/Index");

            return;
        }

        int employeeId = await _tokenServices.ValidateRefreshTokenAsync(token);

        if (employeeId == 0)
        {
            context.Response.Cookies.Delete("jwt");
            context.Response.Cookies.Delete("refreshtoken");

            context.Response.Redirect("/LogIn/Index");
            
            return;
        }

        context.Response.Cookies.Delete("jwt");

        string ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        LoggedInData data = await _logInServices.GetLoggedInDataByIdAsync(employeeId);

        string newAccessToken = _jwtServices.CreateJWToken(data);

        context.Response.Cookies.Append("jwt", newAccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(5)
        });

        await _next(context);
    }
}