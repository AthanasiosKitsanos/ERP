namespace Employees.Shared;

public static class AgeCalculator
{
    public static string CalculateAge(this DateOnly date)
    {
        int age = DateTime.Today.Year - date.Year;

        if (date.AddYears(age) > DateOnly.FromDateTime(DateTime.Today))
        {
            age--;
        }

        return age.ToString();
    }
}
