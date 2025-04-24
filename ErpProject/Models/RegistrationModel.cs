using System;
using ErpProject.Helpers.Connection;
using ErpProject.Services.EmployeeServices;
using ErpProject.Services.AdditionalDetailsServices;
using ErpProject.Services.EmploymentDetailsServices;
using ErpProject.Services.IdentificationServices;
using ErpProject.Services.RoleServices;
using ErpProject.Services.EmployeeCredentialsServices;

namespace ErpProject.Models.RegistrationModel;

public class RegistrationModel
{
    public readonly EmployeeService employeeService;

    public readonly AdditionalDetailsService additionalDetailsService;

    public readonly EmploymentDetailsService employmentDetailsService;

    public IdentificationsService identifivationService;

    public RoleService roleService;

    public EmployeeCredentialsService credentialsService;

    public RegistrationModel(EmployeeService _employeeService, AdditionalDetailsService _additionalDetailsService, EmploymentDetailsService _employmentDetailsService, IdentificationsService _identifivationService, RoleService _roleService, EmployeeCredentialsService _credentialsService)
    {
        employeeService = _employeeService;
        additionalDetailsService = _additionalDetailsService;
        employmentDetailsService = _employmentDetailsService;
        identifivationService = _identifivationService;
        roleService = _roleService;
        credentialsService = _credentialsService;
    }
}
