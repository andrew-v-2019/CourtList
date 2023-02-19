namespace CourtList.Yandex;

public static class Extensions
{
    public static string GetQueryStringParam(this Request request, string paramName)
    {
        if (request?.queryStringParameters == null)
        {
            return string.Empty;
        }

        if (!request.queryStringParameters.ContainsKey(paramName))
        {
            return string.Empty;
        }

        return request.queryStringParameters[paramName].Trim();
    }

    public static int GetPage(this Request request)
    {
        var paramVal = request.GetQueryStringParam(Constants.ParameterName.Page);
        var page = 1;
        if (string.IsNullOrWhiteSpace(paramVal))
        {
            return page;
        }

        int.TryParse(paramVal, out page);
        return page;
    }
}