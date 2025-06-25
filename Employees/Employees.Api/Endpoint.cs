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

    public static class Files
    {
        private const string Base = $"/employees/{{id}}/files";

        public const string GetPhoto = $"{Base}/photograph";
    }

    public static class Credentials
    {
        private const string Base = $"{Employees.Index}/{{id}}/credentials/create";
        public const string Create = Base;
    }

    public static class AdditionalDetails
    {
        private const string Base = $"{Employees.Index}/{{id}}/additionaldetails";
        public const string Get = Base;
        public const string Create = $"{Base}/create";
        public const string Update = $"{Base}/update";
    }

}