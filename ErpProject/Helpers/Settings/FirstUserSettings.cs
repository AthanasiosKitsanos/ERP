using System;
using System.ComponentModel.DataAnnotations;

namespace ErpProject.Helpers.Settings;

public class FirstUserSettings
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Nationality { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets the settings from the settings.json file that is in the .gitignore file
    /// </summary>
    /// <returns>FirstUserSettings class object</returns>
    public static FirstUserSettings GetJsonInfo()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("settings.json", optional: false, reloadOnChange: true).Build();
        var setttings = configuration.GetSection("FirstUserSettings").Get<FirstUserSettings>();

        return setttings!;
    }
}


public class FirstUserEmploymentDetails
{
    public string Position { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string EmploymentStatus { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }

    public string ContractType { get; set; } = string.Empty;

    public string WorkLocation { get; set; } = string.Empty;

    /// <summary>
    /// Gets the settings from the settings.json file that is in the .gitignore file
    /// </summary>
    /// <returns>FirstUserEmploymentDetails object</returns>
    public static FirstUserEmploymentDetails GetJsonInfo()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("settings.json", optional: false, reloadOnChange: true).Build();
        var setttings = configuration.GetSection("FirstUserEmploymentDetails").Get<FirstUserEmploymentDetails>();

        return setttings!;
    }
}

public class FirstUserAdditinalDetails
{
    public string EmergencyNumbers { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;

    [MaxLength(8000)]
    public byte[] Certifications { get; set; } = null!;

    [MaxLength(8000)]
    public byte[] PersonalDocuments { get; set; } = null!;

    public static FirstUserAdditinalDetails GetJsonInfo()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("settings.json", optional: false, reloadOnChange: true).Build();
        var setttings = configuration.GetSection("FirstUserAdditinalDetails").Get<FirstUserAdditinalDetails>();

        return setttings!;
    }
}

public class FirstUserIdentification
{
    public string TIN { get; set; } = string.Empty;

    public bool WorkAuth { get; set; }

    public string TaxInformation { get; set; } = string.Empty;

    public static FirstUserIdentification GetJsonInfo()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("settings.json", optional: false, reloadOnChange: true).Build();
        var setttings = configuration.GetSection("FirstUserIdentification").Get<FirstUserIdentification>();

        return setttings!;
    }
}

public class FirstUserCredentials
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public static FirstUserCredentials GetJsonInfo()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("settings.json", optional: false, reloadOnChange: true).Build();
        var setttings = configuration.GetSection("FirstUserCredentials").Get<FirstUserCredentials>();

        return setttings!;
    }
}
