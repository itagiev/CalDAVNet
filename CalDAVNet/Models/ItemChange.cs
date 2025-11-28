using System.Xml.Linq;

namespace CalDAVNet;

public class ItemChange
{
    private string? _etag;

    public string Href { get; } = null!;

    public int StatusCode { get; }

    public string? Etag
    {
        get
        {
            if (_etag is null
                && Properties.TryGetValue(XNames.GetEtag, out var prop)
                && prop.IsSuccessStatusCode)
            {
                _etag = prop.Prop.Value;
            }

            return _etag;
        }
    }

    public IReadOnlyDictionary<XName, PropResponse> Properties { get; }

    public bool IsDeleted => StatusCode == 404 || StatusCode == 410;

    public bool IsSuccessStatusCode => StatusCode >= 200 && StatusCode <= 299;

    internal ItemChange(MultistatusItem item)
    {
        Href = item.Href;
        StatusCode = item.StatusCode;
        Properties = item.Properties;
    }
}
