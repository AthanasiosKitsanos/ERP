namespace ErpProject.Extentions;

public static class CustomMiddlewareExtention
{
    public static WebApplication AddMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.MapGet("/", context =>
        {
            context.Response.Redirect("/LogIn/Index");
            return Task.CompletedTask;
        });

        return app;
    }
}
