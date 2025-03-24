using System;
using ErpProject.Helpers;
using System.Text;
using ErpProject.ContextDb;
using ErpProject.Models.EmployeeProfile;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ErpProject.Helpers.Settings;
using ErpProject.Helpers.InitializeFolder;
using Microsoft.IdentityModel.Tokens;

namespace ErpProject.Data;

public class SeedData
{
    private readonly ErpDbContext _context;
    private readonly CreateFirstElements _createElements;

    public SeedData(ErpDbContext context, CreateFirstElements createElements)
    {
        _context = context;
        _createElements = createElements;
    }

    /// <summary>
    /// Initializes the database with the first user
    /// </summary>
    /// <returns>void</returns>
    public async Task InitializeAsync()
    {
        await _createElements.CreateRolesAsync();

        // First User Creation
        var settingsEmployee = FirstUserSettings.GetJsonInfo();
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == settingsEmployee.Email);

        if(employee is not null)
        {
            var password = await _context.EmployeeCredentials.Join(_context.Employees, ec => ec.EmployeeId, e => e.Id, (ec, e) => new { ec, e })
                .Where(emp => emp.e.Id == employee.Id)
                .Select(e => e.ec.Password)
                .FirstOrDefaultAsync();

            if(password is not null)
            {
                //var hashedPassword = Hashing.HashPassword(password);

                // if(hashedPassword is not null)
                // {
                //     await _context.EmployeeCredentials.Where(ec => ec.EmployeeId == employee.Id).ExecuteUpdateAsync(ec => ec.SetProperty(e => e.Password, hashedPassword));
                //     await _context.SaveChangesAsync();
                // }
            }
        }
    }
}
