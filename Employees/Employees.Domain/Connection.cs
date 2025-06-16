using Microsoft.Extensions.Configuration;

namespace Employees.Domain;

public class Connection
{
    public string ConnectionString { get; set; }

    public Connection(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
}
