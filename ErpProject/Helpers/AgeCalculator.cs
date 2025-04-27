namespace ErpProject.Helpers;

public class AgeCalculator
{
    public static string CalculateAge(DateOnly dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;

        if(dateOfBirth.AddYears(age) > DateOnly.FromDateTime(DateTime.Today))
        {
            age--;
        }
        
        return age.ToString();
    }
}
