using System.Xml.Linq;

namespace CalDAVNet;

public class Event
{
    private string? _etag;
    private string? _calendarData;

    public string? Etag
    {
        get
        {
            if (_etag is null &&
                Properties.TryGetValue(XNames.GetEtag, out var prop)
                && prop.IsSuccessStatusCode)
            {
                _etag = prop.Prop.Value;
            }

            return _etag;
        }
    }

    public string? CalendarData
    {
        get
        {
            if (_calendarData is null &&
                Properties.TryGetValue(XNames.CalendarData, out var prop)
                && prop.IsSuccessStatusCode)
            {
                _calendarData = prop.Prop.Value;
            }

            return _calendarData;
        }
    }

    public string Href { get; } = null!;

    public IReadOnlyDictionary<XName, PropResponse> Properties { get; }

    public Ical.Net.Calendar? ICalCalendar { get; }

    public Ical.Net.CalendarComponents.CalendarEvent? ICalEvent { get; }

    public Event(MultistatusItem item)
    {
        Href = item.Href;
        Properties = item.Properties;

        if (!string.IsNullOrWhiteSpace(CalendarData))
        {
            ICalCalendar = Ical.Net.Calendar.Load(CalendarData);
            if (ICalCalendar is not null)
            {
                ICalEvent = ICalCalendar.Events.SingleOrDefault();
            }
        }
    }
}
