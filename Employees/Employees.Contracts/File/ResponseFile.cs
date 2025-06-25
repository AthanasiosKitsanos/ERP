namespace Employees.Contracts.File;

public class ResponseFile
{
    public class GetPhoto
    {
        public byte[] Photograph { get; set; } = null!;
        public string Mime { get; set; } = null!;
    }
}
