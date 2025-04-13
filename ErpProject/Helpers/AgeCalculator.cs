namespace ErpProject.Helpers;

public class AgeCalculator
{
    public static string CalculateAge(DateTime dateOfBirth)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;

        if(dateOfBirth.Date > DateTime.Today.AddYears(-age))
        {
            age--;
        }
        
        return age.ToString();
    }
}
