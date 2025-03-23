using System;
using ErpProject.Helpers;
using System.Text;
using ErpProject.ContextDb;
using ErpProject.Models.EmployeeProfile;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ErpProject.Helpers.Settings;
using ErpProject.Helpers.InitializeFolder;

namespace ErpProject.Data;

public class SeedData
{
    private readonly ErpDbContext _context;
    private readonly CreateFirstElements _createFirstElements;

    public SeedData(ErpDbContext context, CreateFirstElements createFirstElements)
    {
        _context = context;
        _createFirstElements = createFirstElements;
    }

    /// <summary>
    /// Initializes the database with the first user
    /// </summary>
    /// <returns>void</returns>
    public async Task InitializeAsync()
    {
        // First User Creation
        var settingsEmployee = FirstUserSettings.GetJsonInfo();
        var employeeExists = await _context.Employees.FirstOrDefaultAsync(e => e.Email == settingsEmployee.Email);

        if(employeeExists is null)
        {
            var employee = new Employee
            {
                FirstName = settingsEmployee.FirstName,
                LastName = settingsEmployee.LastName,
                Email = settingsEmployee.Email,
                DateOfBirth = settingsEmployee.DateOfBirth,
                Nationality = settingsEmployee.Nationality,
                Gender = settingsEmployee.Gender,
                PhoneNumber = settingsEmployee.PhoneNumber,
            };

            employee.Age = AgeCalculator.CalculateAge(employee.DateOfBirth);

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Assigning the role to the employee
            await _createFirstElements.AddRoleToEmployeeAsync(employee, "Admin");

            // Adding more Employement Details
            var settingsDetails = FirstUserEmploymentDetails.GetJsonInfo();

            var employmentDetails = new EmploymentDetails
            {
                Position = settingsDetails.Position,
                Department = settingsDetails.Department,
                EmploymentStatus = settingsDetails.EmploymentStatus,
                HireDate = settingsDetails.HireDate,
                ContractType = settingsDetails.ContractType,
                WorkLocation = settingsDetails.WorkLocation,
                EmployeeId = employee.Id
            };

            await _context.EmploymentDetails.AddAsync(employmentDetails);
            await _context.SaveChangesAsync();

            // Adding more Additional Details
            var settingsAdditionalDetails = FirstUserAdditinalDetails.GetJsonInfo();

            var additionalDetails = new AdditionalDetails
            {
                EmergencyNumbers = settingsAdditionalDetails.EmergencyNumbers,
                Education = settingsAdditionalDetails.Education,
                Certifications = Encoding.ASCII.GetBytes("My Certification"),
                PersonalDocuments = Encoding.ASCII.GetBytes("My Personal Documents"),
                EmployeeId = employee.Id
            };

            await _context.AdditionalDetails.AddAsync(additionalDetails);
            await _context.SaveChangesAsync();

            // TODO: Add the user to the Identity methods to add to the database
            // and create a method that checks if the TIN is a valid number
        }
    }
}
