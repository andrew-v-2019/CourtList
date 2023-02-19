using System.Text.RegularExpressions;

namespace CourtList.Services;

public static class StringExtensions
{
    private static List<Tuple<string, string>> symbols = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>(".", string.Empty),
        new Tuple<string, string>("№", string.Empty),
        new Tuple<string, string>("области", "обл"),
        new Tuple<string, string>("Республика", "Респ")
    };

    private static readonly Regex regex = new Regex(@"\s+");

    public static string ShrinkString(this string str)
    {
        var res = str.Trim();
        foreach (var symbol in symbols)
        {
            if (res.Contains(symbol.Item1))
            {
                res = res.Replace(symbol.Item1, symbol.Item2);
            }
        }

        res = res.Trim().ReplaceWhiteSpaces();
        return res;
    }

    private static string ReplaceWhiteSpaces(this string str)
    {
        return regex.Replace(str, " ");
    }
}