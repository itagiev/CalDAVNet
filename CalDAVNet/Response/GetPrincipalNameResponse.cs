using System.Diagnostics.CodeAnalysis;

namespace CalDAVNet;

public sealed class GetPrincipalNameResponse : ClientResponse
{
    [MemberNotNullWhen(true, nameof(PrincipalName))]
    public override bool IsSuccess => base.IsSuccess;

    public string Href { get; init; } = null!;

    public string? PrincipalName { get; init; }

    internal GetPrincipalNameResponse() { }
}
