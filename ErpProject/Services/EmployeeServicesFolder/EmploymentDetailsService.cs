using System;
using ErpProject.Interfaces.EmployeeInterfaces;
using ErpProject.Models.EmployeeProfile;
using Microsoft.EntityFrameworkCore;
using ErpProject.ContextDb;
using ErpProject.Models.DTOModels.EmployeeDTO;

namespace ErpProject.Services.EmployeeServices;

public class EmploymentDetailsService : IEmploymentDetailsService
{
    private readonly ErpDbContext _dbContext;
    private readonly IEmployeeService _empService;

    public EmploymentDetailsService(ErpDbContext dbContext, IEmployeeService empService)
    {
        _dbContext = dbContext;
        _empService = empService;
    }

    /// <summary>
    /// adds details to the EmploymetDetails table
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="id"></param>
    /// <returns>True if created, else false</returns>
    public async Task<bool> AddEmploymentDetailsAsync(EmploymentDetailsDTO dto, int id)
    {
        var employee = await _empService.GetEmployeeByIdAsync(id);

        if(employee is null)
        {
            return false;
        }

        var newDetails = new EmploymentDetails
        {
            Position = dto.Position,
            Department = dto.Department,
            EmploymentStatus = dto.EmploymentStatus,
            HireDate = dto.HireDate,
            ContractType = dto.ContractType,
            WorkLocation = dto.WorkLocation,
            EmployeeId = employee.Id
        };

        await _dbContext.EmploymentDetails.AddAsync(newDetails);
        await _dbContext.SaveChangesAsync();

        return true;

    }

    /// <summary>
    /// Gets the Employment Details based on the Id of the employee
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An EmployementDetails object</returns>
    public async Task<EmploymentDetails> GetEmploymentDetailsAsync(int id)
    {
        var employmentDetails = await _dbContext.EmploymentDetails.Where(ed => ed.EmployeeId == id).Select(ed => ed).FirstOrDefaultAsync();

        if(employmentDetails is null)
        {
            return null!;
        }

        return employmentDetails;
    }
}
