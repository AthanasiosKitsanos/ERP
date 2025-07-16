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

    public static class AdditionalDetails
    {
        private const string Base = $"/{{id}}/additionaldetails";
        public const string Get = $"{Base}/get";
        public const string Update = $"{Base}/update";
        public const string Create = $"{Base}/create";                                               
    }

    public static class EmploymentDetails
    {
        private const string Base = $"/{{id}}/employmentdetails";
        public const string Get = $"{Base}/get";
        public const string Create = $"{Base}/create";
        public const string Update = $"{Base}/update";
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

        public class AdditionalDetailsViews
        {
            private const string Base = "/additionaldetails";
            public const string Get = $"{Base}/{{id}}/get";
            public const string Update = $"{Base}/{{id}}/update";
            public const string Create = $"{Base}/{{id}}/create";
        }

        public class EmploymentDetailsViews
        {
            private const string Base = "/employmentdetails";
            public const string Get = $"{Base}/get";
            public const string Create = $"{Base}/create";
            public const string Update = $"{Base}/update";
        }
    }
}