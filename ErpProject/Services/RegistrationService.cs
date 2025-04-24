using ErpProject.Helpers.Connection;
using Microsoft.Data.SqlClient;
using ErpProject.Services.EmployeeServices;
using ErpProject.Models.DTOModels;
using ErpProject.Models.RegistrationModel;

namespace ErpProject.Services;

public class RegistrationService
{
    private readonly Connection _connection;

    private readonly RegistrationModel _service;

    public RegistrationService(Connection connection, RegistrationModel service)
    {
        _connection = connection;
        _service = service;
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
                var result = await _service.employeeService.RegisterNewEmployeeAsync(model.Employee, connection, transaction);

                int id = result.id;
                int affectedRows = result.affectedRows;

                if(id <= 0)
                {
                    return false;
                }

                affectedRows += await _service.additionalDetailsService.AddAdditionalDetailsAsync(id, model.AdditionalDetails, connection, transaction);

                affectedRows += await _service.employmentDetailsService.AddEmploymentDetailsAsync(id, model.EmploymentDetails, connection, transaction);

                affectedRows += await _service.additionalDetailsService.AddCertificationsAsync(id, model.Certifications, connection, transaction);

                affectedRows += await _service.additionalDetailsService.AddPersonalDocumentsAsync(id, model.PersonalDocuments, connection, transaction);

                affectedRows += await _service.identifivationService.AddIdentificationsAsync(id, model.Identifications,connection, transaction);

                affectedRows += await _service.roleService.AddRoleToEmployeeAsync(id, model.Roles, connection, transaction);

                affectedRows += await _service.credentialsService.AddCredentialsToEmployeeAsync(id, model.EmployeeCredential, model.AccountStatus, connection, transaction);

                // TODO: Create the rest of the services
                if(affectedRows < 7)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

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