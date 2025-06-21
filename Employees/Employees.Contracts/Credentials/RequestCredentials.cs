using System.ComponentModel.DataAnnotations;

namespace Employees.Contracts.CredentialsContract;

public class RequestCredentials
{
    public class Create
    {
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$";

        public string Username { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [RegularExpression(PasswordPattern, ErrorMessage = "Password must be at least 8 characters, include at least one uppercase letter, one lowercase letter, and one symbol.")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string AccountStatus { get; set; } = string.Empty;

        public int EmployeeId { get; set; }
    }
}
