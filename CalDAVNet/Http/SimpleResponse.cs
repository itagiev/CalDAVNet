namespace CalDAVNet;

public class SimpleResponse
{
    public int StatusCode { get; }

    public bool IsSuccessStatusCode => StatusCode >= 200 && StatusCode <= 299;

    public SimpleResponse(int statusCode)
    {
        StatusCode = statusCode;
    }
}
