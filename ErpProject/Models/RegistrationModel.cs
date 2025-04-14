using System;
using ErpProject.Helpers.Connection;
using ErpProject.Services.EmployeeServices;
using ErpProject.Services.EmployeeServicesFolder;

namespace ErpProject.Models.RegistrationModel;

public class RegistrationModel
{
    public readonly EmployeeService employeeService;

    public readonly AdditionalDetailsService additionalDetailsService;

    public readonly EmploymentDetailsService employmentDetailsService;

    public IdentificationsService identifivationService;

    public RegistrationModel(EmployeeService _employeeService, AdditionalDetailsService _additionalDetailsService, EmploymentDetailsService _employmentDetailsService, IdentificationsService _identifivationService)
    {
        employeeService = _employeeService;
        additionalDetailsService = _additionalDetailsService;
        employmentDetailsService = _employmentDetailsService;
        identifivationService = _identifivationService;
    }
}
