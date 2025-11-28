namespace CalDAVNet;

public class ClientResponse
{
    public ClientResult Result { get; init; }

    public ClientError ErrorCode { get; init; }

    public string? ErrorMessage { get; init; }

    public Exception? Exception { get; init; }

    public virtual bool IsSuccess => Result == ClientResult.Success;

    internal ClientResponse()
    {
    }

    internal ClientResponse(ClientResult result = ClientResult.Success, ClientError errorCode = ClientError.NoError, string? errorMessage = null, Exception? exception = null)
    {
        Result = result;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Exception = exception;
    }
}
