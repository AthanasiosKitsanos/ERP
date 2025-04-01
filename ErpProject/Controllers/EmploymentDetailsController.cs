using System;
using Microsoft.AspNetCore.Mvc;
using ErpProject.Services.EmployeeServices;
using ErpProject.Models.DTOModels.Employee;
using ErpProject.Models.EmployeeProfile;


namespace ErpProject.Controllers;

[Route("employee")]
public class EmploymentDetailsController: Controller
{
   private readonly EmploymentDetails _employmentDetailsService;

   public EmploymentDetailsController(EmploymentDetails employmentDetailsService)
   {
      _employmentDetailsService = employmentDetailsService;
   }

   //[]
}