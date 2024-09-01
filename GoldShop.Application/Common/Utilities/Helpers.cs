using System.Globalization;
using System.Text.RegularExpressions;
using GoldShop.Application.Common.Exceptions;

namespace GoldShop.Application.Common.Utilities;

public static partial class Helpers
{
    // [GeneratedRegex("^\\d+$")]
    // public static partial Regex MyRegex();
    //
    // public static bool IsPhone(this string value)
    // {
    //     return MyRegex().IsMatch(value);
    // }
    public static string GetConfirmCode()
    {
        var code = string.Empty;
        for (var i = 0; i < 6; i++)
        {
            code += new Random().Next(0, 9).ToString();
        }

        return code;
    }
    public static string ToPersianTime(this DateTime calendar)
    {
        try
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", pc.GetYear(calendar), pc.GetMonth(calendar),
                pc.GetDayOfMonth(calendar));
        }
        catch (Exception e)
        {
            return string.Empty;
        }
    }  
}