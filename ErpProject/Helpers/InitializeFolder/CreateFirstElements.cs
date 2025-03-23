using System;
using ErpProject.Helpers.Settings;
using ErpProject.ContextDb;
using ErpProject.Models.EmployeeProfile;
using Microsoft.EntityFrameworkCore;
using ErpProject.Helpers;
using System.Text;

namespace ErpProject.Helpers.InitializeFolder;

public class CreateFirstElements
{
    private readonly ErpDbContext _context;
    private readonly ILogger<CreateFirstElements> _logger;

    public CreateFirstElements(ErpDbContext context, ILogger<CreateFirstElements> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>Creates the fisrt roles in the database</summary>
    /// <returns>void</returns>
    public async Task CreateRolesAsync()
    {
        string[] roles = new string[] { "Owner", "Admin", "Manager", "Employee" };

        foreach(var role in roles)
        {
            var result = await RoleExistsAsync(role);
            
            if(!result)
            {
                await _context.Roles.AddAsync(new Roles { RoleName = role });
                await _context.SaveChangesAsync();
                _logger.LogInformation("Role created");
            }
            else
            {
                _logger.LogInformation("Role already exists");
            }
        }
    }

    /// <summary>
    /// Checks if the role exists in the database
    /// </summary>
    /// <param name="role"></param>
    /// <returns>false if the role does not exist, else it returns true</returns>
    public async Task<bool> RoleExistsAsync(string role)
    {
        var roleExists = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == role);

        if(roleExists is null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Adds a role to an employee
    /// </summary>
    /// <param name="employee"></param>
    /// <param name="role"></param>
    /// <returns>True if the role was added, false if it didn't</returns>
    public async Task<bool> AddRoleToEmployeeAsync(Employee employee, string role)
    {
        var roleId = await GetRoleByIdAsync(role);

        if(roleId <= 0)
        {
            return false;
        }

        var roleEmployee = new RoleEpmloyee
        {
            EmployeeId = employee.Id,
            RoleId = roleId
        };

        await _context.RoleEpmloyee.AddAsync(roleEmployee);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Gets the role id
    /// </summary>
    /// <param name="role"></param>
    /// <returns>True if the roleId was found, else it returns false</returns>
    public async Task<int> GetRoleByIdAsync(string role)
    {
        var roleId = await _context.Roles.Where(r => r.RoleName == role).Select(r => r.Id).FirstOrDefaultAsync();

        if(roleId <= 0)
        {
            return 0;
        }

        return roleId;
    }


}
