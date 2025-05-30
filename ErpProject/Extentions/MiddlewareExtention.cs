namespace ErpProject.Extentions;

public static class CustomMiddlewareExtention
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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/Home/Index");
            return Task.CompletedTask;
        });
 
        return app;
    }
}
