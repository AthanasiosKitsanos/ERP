using System;
using Microsoft.EntityFrameworkCore;
using ErpProject.Models.EmployeeModel;
using ErpProject.Models.AccountStatusModel;
using ErpProject.Models.EmploymentDetailsModel;
using ErpProject.Models.AdditionalDetailsModel;
using ErpProject.Models.IdentificationsModel;
using ErpProject.Models.RolesModel;
using ErpProject.Models.RolesEmployeeModel;
using ErpProject.Models.EmployeeCredentilasModel;
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
    // public DbSet<PayGrades> PayGrades { get; set; }
    // public DbSet<PayGradePerName> PayGradePerName { get; set; }
    // public DbSet<Allowances> Allowances { get; set; }
    // public DbSet<Deductions> Deductions { get; set; }
    // public DbSet<SalaryStructure> SalaryStructure { get; set; }
    // public DbSet<Commissions> Commissions { get; set; }
    // public DbSet<OverTime> OverTime { get; set; }
    // public DbSet<PaymentStatus> PaymentStatus { get; set; }
    // public DbSet<PayrollProseccing> PayrollProseccing { get; set; }
    // public DbSet<PaymentRelations> PaymentRelations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
