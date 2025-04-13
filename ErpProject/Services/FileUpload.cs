namespace ErpProject.Services;

public class FileUpload
{
    private readonly IWebHostEnvironment _env;

    public FileUpload(IWebHostEnvironment env)
    {
        _env = env;
    }

    // public async Task<List<string> UploadFileAsync(List<IFormFile> files)
    // {
    //     if(files is null || files.Count == 0)
    //     {
    //         return null!;
    //     }
        
    //     foreach(IFormFile file in files)
    //     {

    //     }
    // }
}