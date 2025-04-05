using System;
using ErpProject.Helpers.Settings;
using ErpProject.ContextDb;
using Microsoft.EntityFrameworkCore;
using ErpProject.Helpers;
using System.Text;
using ErpProject.Models.RolesModel;
using ErpProject.Models.RolesEmployeeModel;
using ErpProject.Models.EmployeeModel;
using ErpProject.Models.AccountStatusModel;

namespace ErpProject.Helpers.InitializeFolder;

public class CreateFirstElements
{
    private readonly ErpDbContext _context;

    public CreateFirstElements(ErpDbContext context)
    {
        _context = context;
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

    public async Task CreateAccountStatusAsync()
    {
        string[] statusArray = new string[] { "Active", "Inactive", "Blocked" };

        foreach(var status in statusArray)
        {
            var result = await AccountStatusNameExistsAsync(status);

            if(!result)
            {
                await _context.AccountStatus.AddAsync(new AccountStatus { StatusName = status });
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task<bool> AccountStatusNameExistsAsync(string status)
    {
        var statusExists = await _context.AccountStatus.FirstOrDefaultAsync(s => s.StatusName == status);

        if(statusExists is null)
        {
            return false;
        }

        return true;
    }
}
