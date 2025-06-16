using Employees.Api.Extentions;

namespace MainProgram;

class MainProgram
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddJsonFiles();

        var app = builder.Build();

        app.AddMiddleware();

        app.Run();
    }
}