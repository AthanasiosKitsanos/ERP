using System;
using ErpProject.Models.DTOModels.EmployeeDTO;
using ErpProject.Models.EmployeeProfile;

namespace ErpProject.Interfaces;

public interface IEmployeeService
{
    public Task<bool> RegisterNewEmployeeAsync(EmployeeDTO newEmployee);
    public Task<List<Employee>> GetEmployeesAsync();

    public Task<Employee> GetEmployeeByIdAsync(int id);
}
