using System;

namespace ErpProject.Services;

public class PhotoUploadService
{
    private readonly IWebHostEnvironment _env;

    public PhotoUploadService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadPhotoAsync(IFormFile file)
    {
        if(file is null || file.Length == 0)
        {
            return string.Empty;
        }

        if(!file.ContentType.StartsWith("image/"))
        {
            throw new ArgumentException("You can upload only images.");
        }

        string uniquePath = Guid.NewGuid() + Path.GetExtension(file.FileName);

        string uploadFolder = Path.Combine(_env.WebRootPath, "images/ProfilePhotos");
        
        if(!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }
        
        string filePath = Path.Combine(uploadFolder, uniquePath);

        using(FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        string fileUrl = "/images/ProfilePhotos/" + uniquePath;

        return fileUrl;
    }

    public async Task<bool> DeletePhoto(string photoPath)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(photoPath))
            {
                return false;
            }

            string fullPath = Path.Combine(_env.WebRootPath, photoPath.TrimStart('/'));

            return await Task.Run(() =>
            {
                if(File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            });
        }
        catch(Exception)
        {
            throw new  Exception("The file could not be deleted.");
        }
    }
}
