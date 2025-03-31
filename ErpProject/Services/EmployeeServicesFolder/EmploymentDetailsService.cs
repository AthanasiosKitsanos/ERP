using System;
using ErpProject.ContextDb;
using ErpProject.Models.DTOModels.EmployeeDTO;
using Microsoft.EntityFrameworkCore;
using ErpProject.Models.EmployeeProfile;

namespace ErpProject.Services.EmployeeServices;

public class EmploymentDetailsService
{
    private readonly ErpDbContext _dbContext;

    public EmploymentDetailsService(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Add details to the Employment Details Table based in the id
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="id"></param>
    /// <returns>True if there is no any details, else flase</returns>
    public async Task<bool> AddEmploymentDetailsAsync(EmploymentDetailsDTO dto, int id)
    {
        var result = await _dbContext.EmploymentDetails.AnyAsync(ed => ed.EmployeeId == id);

        if(!result)
        {
            var employmentDetails = new EmploymentDetails
            {
                Position = dto.Position,
                Department = dto.Department,
                EmploymentStatus = dto.EmploymentStatus,
                HireDate = dto.HireDate,
                ContractType = dto.ContractType,
                WorkLocation = dto.WorkLocation,
                EmployeeId = id
            };

            await _dbContext.EmploymentDetails.AddAsync(employmentDetails);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Updates elements form the EmploymentDetails Table, depending if they are null or not 
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="id"></param>
    /// <returns>True if there is at least one affected Row, else false</returns>
    public async Task<bool> UpdateEmploymentDetailsAsync(EmploymentDetailsDTO dto, int id)
    {
        var affectedRows = await _dbContext.EmploymentDetails.Where(ed => ed.Id == id)
                           .ExecuteUpdateAsync(ed =>
                           ed.SetProperty(ed => ed.Position, edDTO => string.IsNullOrWhiteSpace(dto.Position) ? edDTO.Position : dto.Position)
                           .SetProperty(ed => ed.Department, edDTO => string.IsNullOrEmpty(dto.Department) ? edDTO.Department : dto.Department)
                           .SetProperty(ed => ed.EmploymentStatus, edDTO => string.IsNullOrEmpty(dto.EmploymentStatus) ? edDTO.EmploymentStatus : dto.EmploymentStatus)
                           .SetProperty(ed => ed.ContractType, edDTO => string.IsNullOrEmpty(dto.ContractType) ? edDTO.ContractType : dto.ContractType)
                           .SetProperty(ed => ed.WorkLocation, edDTO => string.IsNullOrEmpty(dto.WorkLocation) ? edDTO.WorkLocation : dto.WorkLocation));
        
        return affectedRows > 0;
    }

    public async Task<EmploymentDetails> GetEmploymentDetailsAsync(int id)
    {
        var details = await _dbContext.EmploymentDetails.Where(ed => ed.EmployeeId == id).Select(ed => ed).FirstOrDefaultAsync();

        if(details is null)
        {
            return null!;
        }

        return details;
    }
}