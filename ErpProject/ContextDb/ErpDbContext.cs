using System;
using Microsoft.EntityFrameworkCore;
using ErpProject.Models.EmployeeProfile;

namespace ErpProject.ContextDb;

public class ErpDbContext: DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
    {

    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmploymentDetails> EmploymentDetails { get; set; }
    public DbSet<AdditionalDetails> AdditionalDetails { get; set; }
    public DbSet<Identifications> Identifications { get; set; }
    public DbSet<Roles> Roles { get; set; }
    public DbSet<RoleEpmloyee> RoleEpmloyee { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
