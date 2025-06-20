namespace Employees.Api;

public static class Endpoint
{
    public static class LogIn
    {
        public const string LogInPage = "/login";
    }

    public static class Home
    {
        public const string Index = "/home";
    }

    public static class Employees
    {
        private const string Base = "/employees";
        public const string Index = Base;
        public const string GetAllEmployees = $"{Base}/getallemployees";
        public const string Create = $"{Base}/create";
        public const string GetAllDetails = $"{Base}/{{id}}/details";
        public const string GetMainDetails = $"{Base}/{{id}}/getmaindetails";
        public const string Delete = $"{Base}/{{id}}/delete";
        public const string Update = $"{Base}/{{id}}/update";
    }

    public static class Credentials
    {
        private const string Base = $"{Employees.Index}/{{id}}/createCredentials";
        public const string Create = Base;
    }

}
