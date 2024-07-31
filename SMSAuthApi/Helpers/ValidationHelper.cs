using System.Text.RegularExpressions;

namespace Tenge.Service.Helpers;

public static class ValidationHelper
{

    public static bool IsPhoneValid(string phone)
    {
        string pattern = @"^\+998\d{9}$";

        return Regex.IsMatch(phone, pattern);
    }

}
