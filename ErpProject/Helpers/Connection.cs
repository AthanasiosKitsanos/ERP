namespace ErpProject.Helpers.Connection;

public class Connection
{
    public string ConnectionString { get ; set;}

    public Connection(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
}
