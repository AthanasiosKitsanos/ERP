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
        private const string Id = $"/{{id}}";
        public const string GetAllEmployees = $"/getallemployees";
        public const string Get = $"{Id}/getmaindetails"; 
        public const string Update = $"{Get}/update";
    }

    public static class Files
    {
        private const string Base = $"/files/{{id}}";

        public const string GetPhoto = $"{Base}/photograph";
    }

    public static class Credentials
    {
    
    }

    public static class AdditionalDetails
    {
        
    }

    public static class Views
    {
        public class EmployeeViews
        {
            private const string Base = "/employees";
            private const string BaseId = $"{Base}/{{id}}";
            public const string Index = Base;
            public const string Create = $"{Base}/create";
            public const string Details = $"{BaseId}/details";
            public const string GetMainDetails = $"{BaseId}/getmaindetails";
            public const string Update = $"{BaseId}/update";
            public const string Delete = $"{BaseId}/delete";
        }

        public class CredentialsViews
        {
            private const string Base = $"/credentials/{{id}}/create";
            public const string Create = $"{Base}";
        }
    }
}