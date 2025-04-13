using ErpProject.Helpers;
using ErpProject.Models.DTOModels;
using Microsoft.AspNetCore.Mvc;

namespace ErpProject.Controllers;

[Route("additionaldetails")]
public class AdditionalDetailsController: Controller
{
    private readonly FileManagement _fileManagement;

    public AdditionalDetailsController(FileManagement fileManagement)
    {
        _fileManagement = fileManagement;
    }

    [HttpGet("add")]
    public IActionResult Add(ViewModelDTO model)
    {
        if(model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddDetails(ViewModelDTO model)
    {
        if(model is null)
        {
            return RedirectToAction("Add");
        }

        foreach(IFormFile certification in model.CertificationPDF)
        {
            model.AdditionalDetails.CertificationsPath = _fileManagement.UploadCertifications(certification, out string fullPath);

            if(string.IsNullOrEmpty(model.AdditionalDetails.CertificationsPath) || string.IsNullOrWhiteSpace(model.AdditionalDetails.CertificationsPath))
            {
               await _fileManagement.DeleteFile(fullPath);
            }
        }

        foreach(IFormFile document in model.PersonalDocumentsPDF)
        {
            model.AdditionalDetails.PersonalDocumentsPath = _fileManagement.UploadPersonalDocuments(document, out string fullPath);

            if(string.IsNullOrEmpty(model.AdditionalDetails.PersonalDocumentsPath) || string.IsNullOrWhiteSpace(model.AdditionalDetails.PersonalDocumentsPath))
            {
               await _fileManagement.DeleteFile(fullPath);
            }
        }

        return RedirectToAction("Add", "EmploymentDetails", model);
    }
}
