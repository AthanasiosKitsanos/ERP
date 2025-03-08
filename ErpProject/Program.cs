namespace ErpProject;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        

        var app = builder.Build();
    }
}
