using System.Xml.Linq;

namespace CalDAVNet;

public record PropResponse(XElement prop, int statusCode)
{
    public readonly XElement Prop = prop;
    public readonly int StatusCode = statusCode;

    public bool IsSuccessStatusCode => StatusCode >= 200 && StatusCode <= 299;
}
