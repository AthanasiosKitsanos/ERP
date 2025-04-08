using System;
using ErpProject.Helpers.Connection;
using ErpProject.Models.AdditionalDetailsModel;
using Microsoft.Data.SqlClient;

namespace ErpProject.Services.EmployeeServicesFolder;

public class AdditionalDetailsService
{
    private readonly Connection _connection;

    public AdditionalDetailsService(Connection connection)
    {
        _connection = connection;
    }

    public async Task<bool> AddAdditionalDetailsAsync(int id)
    {
        return true;    
    }
}
