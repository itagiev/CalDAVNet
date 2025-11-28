using System.Collections;

namespace CalDAVNet;

public sealed class ClientResponseCollection<TResponse> : IEnumerable<TResponse>
    where TResponse : ClientResponse
{
    private readonly List<TResponse> _responses = new List<TResponse>();
    private ClientResult overallResult;

    public int Count => _responses.Count;

    public TResponse this[int index] => _responses[index];

    public ClientResult OverallResult => overallResult;

    internal ClientResponseCollection()
    {
    }

    internal void Add(TResponse response)
    {
        if (response.Result > overallResult)
        {
            overallResult = response.Result;
        }

        _responses.Add(response);
    }

    public IEnumerator<TResponse> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
