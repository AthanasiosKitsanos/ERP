using ErpProject.Helpers.Connection;

namespace ErpProject.Services;

public class EmploymentDetailsServices
{
    private readonly Connection _connection;

    public EmploymentDetailsServices(Connection connection)
    {
        _connection = connection;
    }

    
}
