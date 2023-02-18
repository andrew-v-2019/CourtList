namespace CourtList.Yandex;

public class Response<T> : ResponseBase
{
    public Response(int statusCode, T body)
    {
        StatusCode = statusCode;
        Body = body;
    }

    public T Body { get; set; }
}