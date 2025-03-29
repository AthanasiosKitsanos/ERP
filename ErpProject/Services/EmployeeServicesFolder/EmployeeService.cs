using System;
using ErpProject.Models.EmployeeProfile;
using ErpProject.Models.DTOModels.EmployeeDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ErpProject.ContextDb;
using ErpProject.Helpers;
using ErpProject.Interfaces.EmployeeInterfaces;

namespace ErpProject.Services.EmployeeServices;

public class EmployeeService: IEmployeeService
{
    private readonly ErpDbContext _dbContext;
    
    public EmployeeService(ErpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Register a new Employee to the Database
    /// </summary>
    /// <param name="newEmployee"></param>
    /// <returns>True, if registration was a success</returns>
    public async Task<bool> RegisterNewEmployeeAsync(EmployeeDTO newEmployee)
    {
        var exists = await _dbContext.Employees.AnyAsync(e => e.Email == newEmployee.Email);

        if(!exists)
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

            return true;
        }

        return false;
    }

    /// <summary>
    /// /// Gets all the elements from the Employee Entity
    /// </summary>
    /// <returns>A List of the Employee Element</returns>
    public async Task<List<Employee>> GetEmployeesAsync()
    {
        var list = await _dbContext.Employees.Select(e => e).ToListAsync();

        if(list is null)
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

        if(employee is null)
        {
            return null!;
        }
        return employee;
    }
}
