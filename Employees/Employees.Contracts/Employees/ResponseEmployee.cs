namespace Employees.Contracts.Employee;

public class ResponseEmployee
{
    public class Get
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Age { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public string Nationality { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public byte[] Photograph { get; set; } = new byte[0];

        public string MIME { get; set; } = string.Empty;
    }
}
