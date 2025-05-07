using ErpProject.Models;

namespace ErpProject.Interfaces;

public interface IEmployeeServices
{
    Task<bool> EmailExistsAsync(string email);

    Task<int> AddEmployeeAsync(Employee employee);

    Task<List<Employee>> GetAllEmployeesAsync();

    Task<bool> DeleteEmployeeByIdAsync(int id);

    Task<Employee> GetEmployeeByIdAsync(int id);
}
