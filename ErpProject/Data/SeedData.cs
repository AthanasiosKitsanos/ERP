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
            // Adding more Additional Details
            var settingsAdditionalDetails = FirstUserAdditinalDetails.GetJsonInfo();

            var addDetailsExist = await _context.AdditionalDetails.AnyAsync(a => a.EmployeeId == employee.Id);

            if(!addDetailsExist)
            {
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
            }

            // TODO: Add the user to the Identity methods to add to the database
            // and create a method that checks if the TIN is a valid number

            var settingsIdentification = FirstUserIdentification.GetJsonInfo();

            var identification = new Identifications
            {
                TIN = settingsIdentification.TIN,
                WorkAuth = settingsIdentification.WorkAuth,
                TaxInformation = settingsIdentification.TaxInformation,
                EmployeeId = employee.Id
            };

            var isValidTin = TinValidation.IsValidTin(identification.TIN);

            if(isValidTin)
            {
                if(!_context.Identifications.Any(i => i.TIN == identification.TIN))
                {
                    await _context.Identifications.AddAsync(identification);
                    await _context.SaveChangesAsync();
                }
            }

            var accountStatus = await _context.AccountStatus.AnyAsync();

            if(!accountStatus)
            {
                await _createElements.CreateAccountStatusAsync();
            }

            var settingsCredentials = FirstUserCredentials.GetJsonInfo();

            var credentials = new EmployeeCredentials
            {
                Username = settingsCredentials.Username,
                Password = settingsCredentials.Password,
                EmployeeId = employee.Id,
                AccountStatusId = await _context.AccountStatus.Where(s => s.StatusName == "Active").Select(s => s.Id).FirstOrDefaultAsync()
            };

            if(!_context.EmployeeCredentials.Any(e => e.Username == credentials.Username))
            {
                await _context.EmployeeCredentials.AddAsync(credentials);
                await _context.SaveChangesAsync();
            }
        }
    }
}
