using System;
using ErpProject.Interfaces;
using ErpProject.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ErpProject.Services;

public class CacheEmployeeServices : IEmployeeServices
{

    private readonly IEmployeeServices _employeeService;

    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheEmployeeServices> _logger;

    public CacheEmployeeServices(IEmployeeServices employeeService, IMemoryCache cache, ILogger<CacheEmployeeServices> logger)
    {
        _employeeService = employeeService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        _logger.LogInformation("Checking if employees are cached...");
        
        return await _cache.GetOrCreateAsync("employees", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            
            return await _employeeService.GetAllEmployeesAsync();

        }) ?? new List<Employee>();
    }

    public async Task<int> AddEmployeeAsync(Employee employee)
    {
        _cache.Remove("employee");
        return await _employeeService.AddEmployeeAsync(employee);
    }

    public async Task<bool> DeleteEmployeeByIdAsync(int id)
    {
        _cache.Remove("employees");
        return await _employeeService.DeleteEmployeeByIdAsync(id);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        _cache.Remove("employees");
        return await _employeeService.EmailExistsAsync(email);
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        _cache.Remove("employees");
        return await _employeeService.GetEmployeeByIdAsync(id);
    }
}
