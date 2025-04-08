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

    // public Task<bool> AddAdditionalDetailsAsync(int id)
    // {
    //     AdditionalDetails details = new AdditionalDetails();

    //     using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
    //     {
    //         string addDetails = @"INSERT INTO AdditionalDetails (Id, EmergencyNumbers, Education, Certification, PersonalDocuments, EmployeeId)
    //                             VALUES ( Id = @Id, EmergencyNumbers = @EmergencyNumbers, Education = @Education, Certification = @Certification, PersonalDocuments = @PersonalDocuments, EmployeeId = @EmployeeId)";
            
    //         using(SqlCommand command = new SqlCommand(addDetails, connection))
    //         {
                
    //         }
    //     }
    // }
}
