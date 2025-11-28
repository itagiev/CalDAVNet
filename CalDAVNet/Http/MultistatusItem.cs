using System.Xml.Linq;

namespace CalDAVNet;

public class MultistatusItem
{
    private readonly Dictionary<XName, PropResponse> _properties;

    public string Href { get; init; } = null!;

    public int StatusCode { get; init; }

    public IReadOnlyDictionary<XName, PropResponse> Properties => _properties;

    public MultistatusItem(string href, Dictionary<XName, PropResponse> properties, int statusCode = 200)
    {
        Href = href;
        _properties = properties;
        StatusCode = statusCode;
    }

    public bool IsSuccessStatusCode => StatusCode >= 200 && StatusCode <= 299;

    public bool IsCalendar => Properties.TryGetValue(XNames.ResourceType, out var prop)
        && prop.IsSuccessStatusCode
        && prop.Prop.Element(XNames.Calendar) is not null;
}
