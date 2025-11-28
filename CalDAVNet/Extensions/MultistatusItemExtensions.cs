namespace CalDAVNet;

public static class MultistatusItemExtensions
{
    public static GetPrincipalNameResponse ToGetPrincipalNameResponse(this MultistatusItem item)
    {
        return new GetPrincipalNameResponse
        {
            Href = item.Href,
            PrincipalName = (item.IsSuccessStatusCode
                && item.Properties.TryGetValue(XNames.CurrentUserPrincipal, out var principal)
                && principal.IsSuccessStatusCode)
                ? principal.Prop.Value
                : null,
            Result = item.IsSuccessStatusCode ? ClientResult.Success : ClientResult.Error,
            ErrorCode = ResponseMappings.StatusCodeToClientError(item.StatusCode),
            ErrorMessage = ResponseMappings.StatusCodeToErrorMessage(item.StatusCode),
        };
    }

    public static GetPrincipalResponse ToGetPrincipalResponse(this MultistatusItem item)
    {
        return new GetPrincipalResponse
        {
            Href = item.Href,
            Principal = item.IsSuccessStatusCode ? new Principal(item) : null,
            Result = item.IsSuccessStatusCode ? ClientResult.Success : ClientResult.Error,
            ErrorCode = ResponseMappings.StatusCodeToClientError(item.StatusCode),
            ErrorMessage = ResponseMappings.StatusCodeToErrorMessage(item.StatusCode),
        };
    }

    public static GetCalendarResponse ToGetCalendarResponse(this MultistatusItem item)
    {
        return new GetCalendarResponse()
        {
            Href = item.Href,
            Calendar = item.IsSuccessStatusCode ? new Calendar(item) : null,
            Result = item.IsSuccessStatusCode ? ClientResult.Success : ClientResult.Error,
            ErrorCode = ResponseMappings.StatusCodeToClientError(item.StatusCode),
            ErrorMessage = ResponseMappings.StatusCodeToErrorMessage(item.StatusCode),
        };
    }

    public static GetEventResponse ToGetEventResponse(this MultistatusItem item)
    {
        return new GetEventResponse()
        {
            Href = item.Href,
            Event = item.IsSuccessStatusCode ? new Event(item) : null,
            Result = item.IsSuccessStatusCode ? ClientResult.Success : ClientResult.Error,
            ErrorCode = ResponseMappings.StatusCodeToClientError(item.StatusCode),
            ErrorMessage = ResponseMappings.StatusCodeToErrorMessage(item.StatusCode),
        };
    }
}
