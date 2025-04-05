using System;

namespace ErpProject.Models.Connection;

public class Connection
{
    public string ConnectionString { get ; set;}

    public Connection(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
}
