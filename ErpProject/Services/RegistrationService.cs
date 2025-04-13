using ErpProject.Helpers.Connection;
using Microsoft.Data.SqlClient;
using ErpProject.Services.EmployeeServices;
using ErpProject.Models.DTOModels;
using ErpProject.Services.EmployeeServicesFolder;

namespace ErpProject.Services;

public class RegistrationService
{
    private readonly Connection _connection;
    private readonly EmployeeService _eServices;
    private readonly AdditionalDetailsService _adService;
    private readonly EmploymentDetailsService _edService;

    public RegistrationService(Connection connection, EmployeeService eServices, AdditionalDetailsService adService, EmploymentDetailsService edService)
    {
        _connection = connection;
        _eServices = eServices;
        _adService = adService;
        _edService = edService;
    }

    /// <summary>
    /// It completes the full registration of an employee
    /// </summary>
    /// <param name="model">A ViewModelDTO that uses all the Models of the DTO folder as properties</param>
    /// <returns>True if the transaction is completed, false if an error occures</returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> RegistrationCompleteAsync(ViewModelDTO model)
    {
        if(model is null)
        {
            return false;
        }

        using(SqlConnection connection = new SqlConnection(_connection.ConnectionString))
        {
            await connection.OpenAsync();

            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                int id = await _eServices.RegisterNewEmployeeAsync(model.Employee, connection, transaction);

                if(id == 0)
                {
                    return false;
                }

                await _adService.AddAdditionalDetailsAsync(id, model.AdditionalDetails, connection, transaction);

                await _edService.AddEmploymentDetailsAsync(id, model.EmploymentDetail, connection, transaction);

                await transaction.CommitAsync();
                return true;
            }
            catch(SqlException ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Sql Error: {ex.Message}");
            }
            catch(Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}