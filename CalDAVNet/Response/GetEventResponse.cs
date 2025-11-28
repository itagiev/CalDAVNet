using System.Diagnostics.CodeAnalysis;

namespace CalDAVNet;

public sealed class GetEventResponse : ClientResponse
{
    [MemberNotNullWhen(true, nameof(Event))]
    public override bool IsSuccess => base.IsSuccess;

    public string Href { get; init; } = null!;

    public Event? Event { get; init; }

    internal GetEventResponse() { }
}
