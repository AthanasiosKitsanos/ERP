namespace ErpProject.Helpers;

public class FileToByteArray
{
    public long ArrayLength { get; set; }

    public async Task<byte[]> AddToArray(IFormFile file)
    {
        ArrayLength = file.Length;

        byte[] array = new byte[ArrayLength];

        using(MemoryStream stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            array = stream.ToArray();
        }

        return array;
    }
}
