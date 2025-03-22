using System;
using ErpProject.Helpers;
using System.Text;
using ErpProject.ContextDb;
using ErpProject.Models.EmployeeProfile;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ErpProject.Data;

public class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        // var context = services.GetRequiredService<ErpDbContext>();

        // string[] roles = new string[] { "Owner", "Admin", "Manager", "Employee" };

        // foreach(var role in roles)
        // {
        //     var roleExists = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == role);

        //     if(roleExists is null)
        //     {
        //         await context.Roles.AddAsync(new Roles { RoleName = role });
        //         await context.SaveChangesAsync();
        //     }
        // }



        // List<AccountStatus> accountStatus = context.AccountStatus.ToList();
        // foreach(var status in accountStatus)
        // {
        //     context.AccountStatus.Remove(status);
        //     await context.SaveChangesAsync();
        // }

        // List<AdditionalDetails> additionalDetails = context.AdditionalDetails.ToList();
        // foreach(var details in additionalDetails)
        // {
        //     context.AdditionalDetails.Remove(details);
        //     await context.SaveChangesAsync();
        // }

        // List<Employee> employees = context.Employees.ToList();
        // foreach(var emp in employees)
        // {
        //     context.Employees.Remove(emp);
        //     await context.SaveChangesAsync();
        // }

        // List<EmployeeCredentials> employeeCredentials = context.EmployeeCredentials.ToList();
        // foreach(var cred in employeeCredentials)
        // {
        //     context.EmployeeCredentials.Remove(cred);
        //     await context.SaveChangesAsync();
        // }

        // List<EmploymentDetails> employmentDetails = context.EmploymentDetails.ToList();
        // foreach(var empDetails in employmentDetails)
        // {
        //     context.EmploymentDetails.Remove(empDetails);
        //     await context.SaveChangesAsync();
        // }

        // List<Identifications> identifications = context.Identifications.ToList();
        // foreach(var id in identifications)
        // {
        //     context.Identifications.Remove(id);
        //     await context.SaveChangesAsync();
        // }
        // List<RoleEpmloyee> roleEpmloyee = context.RoleEpmloyee.ToList();
        // foreach(var roleEmp in roleEpmloyee)
        // {
        //     context.RoleEpmloyee.Remove(roleEmp);
        //     await context.SaveChangesAsync();
        // }
        
        // List<Roles> roles = context.Roles.ToList();
        // foreach(var role in roles)
        // {
        //     context.Roles.Remove(role);
        //     await context.SaveChangesAsync();
        // }
    }
}
