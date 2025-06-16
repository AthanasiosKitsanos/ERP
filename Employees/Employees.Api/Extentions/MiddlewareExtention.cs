namespace Employees.Api.Extentions;

public static class MiddlewareExtention
{
    public static WebApplication AddMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseHttpMethodOverride(new HttpMethodOverrideOptions
        {
            FormFieldName = "_method"
        });

        app.UseRouting();
        //app.UseMiddleware<RefreshTokenMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.MapGet("/", context =>
        {
            context.Response.Redirect("/home/index");
            return Task.CompletedTask;
        });

        return app;
    }
}
