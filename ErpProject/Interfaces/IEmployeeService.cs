using System;
using ErpProject.Models.DTOModels.EmployeeDTO;

namespace ErpProject.Interfaces;

public interface IEmployeeService
{
    public Task<bool> RegisterNewEmployeeAsync(EmployeeDTO newEmployee);
}
