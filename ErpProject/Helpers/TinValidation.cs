namespace ErpProject.Helpers;

public static class TinValidation
{
    public static async Task<bool> IsValidTin(string tin)
    {

        var isValid = await ValidateNumberIntegrity(tin);

        if(!isValid)
        {
            return false;
        }

        int sum = 0;
        int power = 8;

        for(int i = 0; i < tin.Length - 1; i++)
        {
            sum = sum + (tin[i] -'0') * Convert.ToInt32(Math.Pow(2, power));
            power--;
        }

        int remainder = sum / 11;
        int lastTinDigit = sum - (remainder * 11);

        if(lastTinDigit != tin[8] - '0')
        {
            return false;
        }

        return true;
    }

    public static async Task<bool> ValidateNumberIntegrity(string tinNumber)
    {   
        if(string.IsNullOrEmpty(tinNumber) || string.IsNullOrWhiteSpace(tinNumber))
        {
            return false;
        }

        await Task.Run(() =>
        {
            if(!int.TryParse(tinNumber, out _) || tinNumber.Length != 9)
            {
                return false;
            }

            int count = 0;

            for(int i = 0; i < tinNumber.Length; i++)
            {
                if(tinNumber[i] == '0')
                {
                    count++;
                }
            }

            if(count >= 9)
            {
                return false;
            }

            return true;
        });

        return true;
    }
}
