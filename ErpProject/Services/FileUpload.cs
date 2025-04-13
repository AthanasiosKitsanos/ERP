namespace ErpProject.Services;

public class FileUpload
{
    private readonly IWebHostEnvironment _env;

    public FileUpload(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
    {
        if(file is null || file.Length == 0)
        {
            return null!;
        }

        using(MemoryStream stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
    }
}