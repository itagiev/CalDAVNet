using System.Diagnostics.CodeAnalysis;

namespace CalDAVNet;

public sealed class ClientItemResponse<TItem> : ClientResponse
{
    [MemberNotNullWhen(true, nameof(Item))]
    public override bool IsSuccess => base.IsSuccess;

    public TItem? Item { get; }

    internal ClientItemResponse(TItem? item)
    {
        Item = item;
    }

    internal ClientItemResponse(ClientResult result = ClientResult.Success, ClientError errorCode = ClientError.NoError, string? errorMessage = null, Exception? exception = null)
        : base(result, errorCode, errorMessage, exception)
    {
    }
}
