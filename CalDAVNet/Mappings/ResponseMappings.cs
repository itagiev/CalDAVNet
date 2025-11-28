namespace CalDAVNet;

public static class ResponseMappings
{
    public static ClientError StatusCodeToClientError(int statusCode)
        => statusCode switch
        {
            >=200 and <= 299 => ClientError.NoError,
            404 => ClientError.NotFound,
            _ => ClientError.Failure
        };

    public static string? StatusCodeToErrorMessage(int statusCode)
        => statusCode switch
        {
            >= 200 and <= 299 => null,
            404 => "Resource not found.",
            _ => "Failed to get resource."
        };
}
