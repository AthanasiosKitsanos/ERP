using ErpProject.Extentions;

namespace MainProgram;

class MainProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddJsonFiles();

        builder.Services.AddCustomServices();

        builder.Services.AddDISetup();

        var app = builder.Build();

        app.AddMiddleware();
   
        app.Run();
    }
}