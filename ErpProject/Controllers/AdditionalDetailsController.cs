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
            return RedirectToAction("Add", model);
        }



        foreach(IFormFile certification in model.CertificationPDF)
        {
            var result = await _fileManagement.UploadCertificationsAsync(certification);

            model.Certifications.CertificationPaths.Add(result.CertificateUrl);

            string fullPath = result.FullPath;

            if(model.Certifications.CertificationPaths is null || model.Certifications.CertificationPaths.Count == 0)
            {
               await _fileManagement.DeleteFile(fullPath);

               return View(model);
            }
        }

        foreach(IFormFile document in model.PersonalDocumentsPDF)
        {
            var result = await _fileManagement.UploadPersonalDocumentsAsync(document);

            model.PersonalDocuments.DocumentsPaths.Add(result.DocumentUrl);

            string fullPath = result.FullPath;

            if(model.PersonalDocuments.DocumentsPaths is null ||  model.PersonalDocuments.DocumentsPaths.Count == 0)
            {
               await _fileManagement.DeleteFile(fullPath);

               return View(model);
            }
        }

        return RedirectToAction("Add", "EmploymentDetails", model);
    }
}
