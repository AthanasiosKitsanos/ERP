namespace ErpProject.Extentions;

public static class WebApplicationBuilderExtention
{
    public static WebApplicationBuilder AddJsonFiles(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("demoSecretKey.json", optional: false, reloadOnChange: true);
        
        builder.Configuration.AddJsonFile("header.json", optional: false, reloadOnChange: true);

        return builder;
    }
}
