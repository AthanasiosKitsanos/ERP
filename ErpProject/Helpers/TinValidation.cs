using System;
using System.ComponentModel.DataAnnotations;

namespace ErpProject.Helpers;

public static class TinValidation
{
    public static bool IsValidTin(string tin)
    {
        var isValid = ValidateNumberIntegrity(tin);

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

    public static bool ValidateNumberIntegrity(string tinNumber)
    {
        int count = 0;

        bool isValidString = int.TryParse(tinNumber, out _);

        for(int i = 0; i < tinNumber.Length; i++)
        {
            if(tinNumber[i] == '0')
            {
                count++;
            }
        }

        if(!isValidString || tinNumber.Length != 9 || count == 9)
        {
            return false;
        }

        return true;
    }
}
