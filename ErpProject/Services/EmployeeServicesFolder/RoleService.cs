using System;
using Microsoft.EntityFrameworkCore;
using ErpProject.ContextDb;
using ErpProject.Models.EmployeeProfile;

namespace ErpProject.Services.RoleServices;

public class RoleService
{
    private readonly ErpDbContext _context;

    public RoleService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<List<Roles>> GetAllRolesAsync()
    {
        var roleList = await _context.Roles.Select(r => r).ToListAsync();

        if(roleList is null)
        {
            return null!;
        }
    
        return roleList;
    }

     /// <summary>
    /// Adds a role to the Employee
    /// </summary>
    /// <param name="employee">Param for an Employee object</param>
    /// <param name="roleName">The role's name</param>
    /// <returns>True of the role is added. else false</returns>
    public async Task<bool> AddRoleToEmployeeAsync(int id, string roleName)
    {
        var roleId = await _context.Roles.Where(r => r.RoleName == roleName).Select(r => r.Id).FirstOrDefaultAsync();

        if(roleId <= 0)
        {
            return false;
        }

        var roleToEmployee = new RoleEpmloyee
        {
            EmployeeId = id,
            RoleId = roleId
        };

        await _context.RoleEpmloyee.AddAsync(roleToEmployee);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }
}
