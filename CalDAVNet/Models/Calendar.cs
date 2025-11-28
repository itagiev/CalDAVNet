using System.Xml.Linq;

namespace CalDAVNet;

public class Calendar
{
    private string? _displayName;
    private string? _description;
    private string? _color;
    private string? _ctag;
    private string? _syncToken;
    private CalendarComponent _supportedCalendarComponentSet = CalendarComponent.None;

    public string? DisplayName
    {
        get
        {
            if (_displayName is null
                && Properties.TryGetValue(XNames.DisplayName, out var prop)
                && prop.IsSuccessStatusCode)
            {
                _displayName = prop.Prop.Value;
            }

            return _displayName;
        }
    }

    public string? Description
    {
        get
        {
            if (_description is null
                && Properties.TryGetValue(XNames.CalendarDescription, out var prop)
                && prop.IsSuccessStatusCode)
            {
                _description = prop.Prop.Value;
            }

            return _description;
        }
    }

    public string? Color
    {
        get
        {
            if (_color is null
                && Properties.TryGetValue(XNames.CalendarColor, out var prop)
                && prop.IsSuccessStatusCode)
            {
                _color = prop.Prop.Value;
            }

            return _color;
        }
    }

    public string? Ctag
    {
        get
        {
            if (_ctag is null
                && Properties.TryGetValue(XNames.GetCtag, out var prop)
                && prop.IsSuccessStatusCode)
            {
                _ctag = prop.Prop.Value;
            }

            return _ctag;
        }
    }

    public string? SyncToken
    {
        get
        {
            if (_syncToken is
                null && Properties.TryGetValue(XNames.SyncToken, out var prop)
                && prop.IsSuccessStatusCode)
            {
                _syncToken = prop.Prop.Value;
            }

            return _syncToken;
        }
    }

    public CalendarComponent SupportedCalendarComponentSet
    {
        get
        {
            if (_supportedCalendarComponentSet == CalendarComponent.None
                && Properties.TryGetValue(XNames.SupportedCalendarComponentSet, out var prop)
                && prop.IsSuccessStatusCode)
            {
                foreach (var comp in prop.Prop.Elements(XNames.Comp))
                {
                    if (comp.Attribute("name") is XAttribute attr)
                    {
                        switch (attr.Value)
                        {
                            case Constants.Comp.VEVENT:
                                _supportedCalendarComponentSet |= CalendarComponent.VEVENT;
                                break;
                            case Constants.Comp.VTODO:
                                _supportedCalendarComponentSet |= CalendarComponent.VTODO;
                                break;
                            case Constants.Comp.VJOURNAL:
                                _supportedCalendarComponentSet |= CalendarComponent.VJOURNAL;
                                break;
                            case Constants.Comp.VFREEBUSY:
                                _supportedCalendarComponentSet |= CalendarComponent.VFREEBUSY;
                                break;
                            case Constants.Comp.VALARM:
                                _supportedCalendarComponentSet |= CalendarComponent.VALARM;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return _supportedCalendarComponentSet;
        }
    }

    public string Href { get; } = null!;

    public IReadOnlyDictionary<XName, PropResponse> Properties { get; }

    public Calendar(MultistatusItem item)
    {
        Href = item.Href;
        Properties = item.Properties;
    }

    public bool IsComponentSupported(CalendarComponent component)
    {
        return (component & SupportedCalendarComponentSet) == component;
    }
}
