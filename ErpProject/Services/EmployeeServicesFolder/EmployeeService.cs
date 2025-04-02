using System;
using ErpProject.Models.EmployeeProfile;
using ErpProject.Models.DTOModels.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ErpProject.ContextDb;
using ErpProject.Helpers;

namespace ErpProject.Services.EmployeeServices;

public class EmployeeService
{
    private readonly ErpDbContext _dbContext;

    public EmployeeService(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Registers a new Employee to the Employee Table
    /// </summary>
    /// <param name="newEmployee"></param>
    /// <returns>True, if registration was a success</returns>
    public async Task<int> RegisterNewEmployeeAsync(RegisterDTO newEmployee)
    {
        if(newEmployee is null)
        {
            return -1;
        }

        var exists = await _dbContext.Employees.AnyAsync(e => e.Email == newEmployee.Email);

        if (!exists)
        {
            var newEntry = new Employee
            {
                FirstName = newEmployee.FirstName,
                LastName = newEmployee.LastName,
                Email = newEmployee.Email,
                DateOfBirth = newEmployee.DateOfBirth,
                Nationality = newEmployee.Nationality,
                Gender = newEmployee.Gender,
                PhoneNumber = newEmployee.PhoneNumber
            };

            newEntry.Age = AgeCalculator.CalculateAge(newEntry.DateOfBirth);

            await _dbContext.Employees.AddAsync(newEntry);
            await _dbContext.SaveChangesAsync();

            return newEntry.Id;
        }

        return -1;
    }

    /// <summary>
    /// /// Gets all the elements from the Employee Table
    /// </summary>
    /// <returns>A List of the Employee Element</returns>
    public async Task<List<Employee>> GetEmployeesAsync()
    {
        var list = await _dbContext.Employees.Select(e => e).ToListAsync();

        if (list is null)
        {
            return null!;
        }

        return list;
    }

    /// <summary>
    /// Gets an Employee element, specified by the id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An Employee class</returns>
    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        var employee = await _dbContext.Employees.Where(e => e.Id == id).Select(e => e).FirstOrDefaultAsync();

        if (employee is null)
        {
            return null!;
        }
        return employee;
    }

    /// <summary>
    /// Updated the Email and PhoneNumber of an Employee
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="id"></param>
    /// <returns>True if at least one Row is affected</returns>
    public async Task<bool> UpdateEmployeeAsync(UpdateDTO dto, int id)
    {
        if(dto == null)
        {
            return false;
        }

        var affectedRows = await _dbContext.Employees
            .Where(emp => emp.Id == id)
            .ExecuteUpdateAsync(e => e
                .SetProperty(emp => emp.Email, empdto => string.IsNullOrEmpty(dto.Email) ? empdto.Email : dto.Email)
                .SetProperty(emp => emp.PhoneNumber, empdto => string.IsNullOrEmpty(dto.PhoneNumber) ? empdto.PhoneNumber : dto.PhoneNumber)
            );

        return affectedRows > 0;
    }

    /// <summary>
    /// Deletes the employee and every relation with it
    /// </summary>
    /// <param name="id">The id of the Employee</param>
    /// <returns>True if deletion was a success, else false</returns>
    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        if(id <= 0)
        {
            return false;
        }

        int affectedRows = await _dbContext.Employees.Where(e => e.Id == id).ExecuteDeleteAsync();

        return affectedRows > 0;
    }
}
