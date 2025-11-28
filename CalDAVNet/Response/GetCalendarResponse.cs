using System.Diagnostics.CodeAnalysis;

namespace CalDAVNet.Response;

public sealed class GetCalendarResponse : ClientResponse
{
    [MemberNotNullWhen(true, nameof(Calendar))]
    public override bool IsSuccess => base.IsSuccess;

    public string Href { get; init; } = null!;

    public Calendar? Calendar { get; init; }

    internal GetCalendarResponse()
    {
    }
}
