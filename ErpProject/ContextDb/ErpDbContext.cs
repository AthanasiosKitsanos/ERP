using System;
using Microsoft.EntityFrameworkCore;
using ErpProject.Models.EmployeeProfile;
using ErpProject.Models.Payments;

namespace ErpProject.ContextDb;

public class ErpDbContext: DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
    {

    }

    /// <summary>Employee Profile</summary>
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmploymentDetails> EmploymentDetails { get; set; }
    public DbSet<AdditionalDetails> AdditionalDetails { get; set; }
    public DbSet<Identifications> Identifications { get; set; }
    public DbSet<Roles> Roles { get; set; }
    public DbSet<RoleEpmloyee> RoleEpmloyee { get; set; }
    public DbSet<EmployeeCredentials> EmployeeCredentials { get; set; }
    public DbSet<AccountStatus> AccountStatus { get; set; }

    /// <summary>Payments</summary>
    public DbSet<PayGrades> PayGrades { get; set; }
    public DbSet<PayGradePerName> PayGradePerName { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
