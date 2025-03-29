using System;
using ErpProject.Models.EmployeeProfile;
using ErpProject.Models.DTOModels.EmployeeDTO;

namespace ErpProject.Interfaces.EmployeeInterfaces;

public interface IEmploymentDetailsService
{
    public Task<bool> AddEmploymentDetailsAsync(EmploymentDetailsDTO dto, int id);

    public Task<EmploymentDetails> GetEmploymentDetailsAsync(int id);
}
