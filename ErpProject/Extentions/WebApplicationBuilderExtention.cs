namespace ErpProject.Extentions;

public static class WebApplicationBuilderExtention
{
    public static WebApplicationBuilder AddJsonFiles(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("DemoKey.json", optional: false, reloadOnChange: true);
        
        builder.Configuration.AddJsonFile("JWTHeader.json", optional: false, reloadOnChange: true);

        return builder;
    }
}
