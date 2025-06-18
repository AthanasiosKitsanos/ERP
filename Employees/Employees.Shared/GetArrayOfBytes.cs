using Microsoft.AspNetCore.Http;

namespace Employees.Shared;

public static class ByteArray
{
    public static async Task<byte[]> GetArrayOfBytes(this IFormFile file)
    {
        long arrayLength = file.Length;

        byte[] photograph = new byte[arrayLength];

        await using (MemoryStream stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            photograph = stream.ToArray();
        }

        return photograph;
    }
}