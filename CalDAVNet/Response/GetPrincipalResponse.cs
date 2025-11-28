using System.Diagnostics.CodeAnalysis;

namespace CalDAVNet;

public sealed class GetPrincipalResponse : ClientResponse
{
    [MemberNotNullWhen(true, nameof(Principal))]
    public override bool IsSuccess => base.IsSuccess;

    public string Href { get; init; } = null!;

    public Principal? Principal { get; init; }

    internal GetPrincipalResponse() { }
}
