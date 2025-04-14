using System;

namespace ErpProject.Helpers;

public class FileManagement
{
    private readonly IWebHostEnvironment _env;

    public FileManagement(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<(string ProfilePhotoUrl, string FullPath)> UploadPhotoAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return (string.Empty, string.Empty);
        }

        if (!file.ContentType.StartsWith("image/"))
        {
            throw new ArgumentException("You can upload only images.");
        }

        string uniquePath = Guid.NewGuid() + Path.GetExtension(file.FileName);

        string uploadFolder = Path.Combine(_env.WebRootPath, "images/ProfilePhotos");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        string fullPath = Path.Combine(uploadFolder, uniquePath);

        try
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string profilePhotoUrl = "/images/ProfilePhotos/" + uniquePath;

            return (profilePhotoUrl, fullPath);
        }
        catch (Exception)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            throw;
        }
    }

    public async Task<(string CertificateUrl, string FullPath)> UploadCertificationsAsync(IFormFile certificationFile)
    {
        if (!certificationFile.ContentType.StartsWith("application/pdf"))
        {
            throw new ArgumentException("You can upload only pdf files");
        }

        string uniqueCertificationPath = Guid.NewGuid() + Path.GetExtension(certificationFile.FileName);

        string uploadFolder = Path.Combine(_env.WebRootPath, "Documents/Certifications");

        if (!File.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        string fullPath = Path.Combine(uploadFolder, uniqueCertificationPath);

        try
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await certificationFile.CopyToAsync(stream);
            }

            string certificateUrl = "Documents/Certifications/" + uniqueCertificationPath;

            return (certificateUrl, fullPath);
        }
        catch (Exception)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            throw;
        }
    }

    public async Task<(string DocumentUrl, string FullPath)> UploadPersonalDocumentsAsync(IFormFile certificationFile)
    {
        if (!certificationFile.ContentType.StartsWith("application/pdf"))
        {
            throw new ArgumentException("You can upload only pdf files");
        }

        string uniqueCertificationPath = Guid.NewGuid() + Path.GetExtension(certificationFile.FileName);

        string uploadFolder = Path.Combine(_env.WebRootPath, "Documents/Personal Documents");

        if (!File.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        string fullPath = Path.Combine(uploadFolder, uniqueCertificationPath);

        try
        {
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await certificationFile.CopyToAsync(stream);

            }

            string certificateUrl = "Documents/Personal Documents/" + uniqueCertificationPath;

            return (certificateUrl, fullPath);
        }
        catch (Exception)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            throw;
        }
    }

    public async Task DeleteFile(string path)
    {
        await Task.Run(() =>
        {
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        });
        
    }
}
