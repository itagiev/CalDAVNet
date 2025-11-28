using System.Text;
using System.Xml.Linq;

using CalDAVNet.Response;

namespace CalDAVNet;

public class CalDAVClient : ICalDAVClient
{
    private readonly HttpClient _httpClient;

    public CalDAVClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MultistatusResponse> SendForMultistatusAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        return new MultistatusResponse(content);
    }

    public async Task SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();
    }

    public async Task<ClientItemResponse<string>> GetPrincipalNameAsync(CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(ICalDAVClient.Propfind, "")
        {
            Content = BodyHelper.CurrentUserPrincipalPropfind().ToStringContent()
        }
        .WithDepth(0)
        .WithPrefer("return-minimal");

        var response = await SendForMultistatusAsync(request, cancellationToken).ConfigureAwait(false);

        return (response.FirstOrDefault() is MultistatusItem item
            && item.Properties.TryGetValue(XNames.CurrentUserPrincipal, out var principal)
            && principal.IsSuccessStatusCode)
            ? new ClientItemResponse<string>(principal.Prop.Value)
            : new ClientItemResponse<string>(ClientResult.Error, ClientError.Failure, "Failed to retrieve principal name.");
    }

    public async Task<ClientItemResponse<Principal>> GetPrincipalAsync(string upn, CancellationToken cancellationToken = default)
    {
        var body = BodyHelper.AllPropPropfind();

        var request = new HttpRequestMessage(ICalDAVClient.Propfind, upn)
            .WithDepth(0)
            .WithPrefer("return-minimal");

        var response = await SendForMultistatusAsync(request, cancellationToken).ConfigureAwait(false);

        return (response.FirstOrDefault() is MultistatusItem item && item.IsSuccessStatusCode)
            ? new ClientItemResponse<Principal>(new Principal(item))
            : new ClientItemResponse<Principal>(ClientResult.Error, ClientError.Failure, $"Failed to retrieve principal.");
    }

    public async Task<ClientResponseCollection<ClientItemResponse<Calendar>>> GetCalendarsAsync(string href, XDocument body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(ICalDAVClient.Propfind, href)
        {
            Content = body.ToStringContent()
        }
        .WithDepth(1)
        .WithPrefer("return-minimal");

        var response = await SendForMultistatusAsync(request, cancellationToken).ConfigureAwait(false);

        ClientResponseCollection<ClientItemResponse<Calendar>> result = [];

        foreach (var item in response)
        {
            if (!item.IsCalendar)
            {
                continue;
            }

            if (item.IsSuccessStatusCode)
            {
                result.Add(new ClientItemResponse<Calendar>(new Calendar(item)));
            }
            else
            {
                result.Add(new ClientItemResponse<Calendar>(ClientResult.Error, ClientError.Failure, $"Failed to retrieve calendars."));
            }
        }

        return result;
    }

    public async Task<ClientItemResponse<Calendar>> GetCalendarAsync(string href, XDocument body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(ICalDAVClient.Propfind, href)
        {
            Content = body.ToStringContent()
        }
        .WithDepth(0)
        .WithPrefer("return-minimal");

        var response = await SendForMultistatusAsync(request, cancellationToken).ConfigureAwait(false);

        return (response.FirstOrDefault() is MultistatusItem item
            && item.IsCalendar
            && item.IsSuccessStatusCode)
            ? new ClientItemResponse<Calendar>(new Calendar(item))
            : new ClientItemResponse<Calendar>(ClientResult.Error, ClientError.Failure, $"Failed to retrieve calendar.");
    }

    public Task CreateCalendarAsync(string href, XDocument body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(ICalDAVClient.Mkcalendar, $"{href}events/")
        {
            Content = body.ToStringContent()
        }
        .WithPrefer("return=representation");

        return SendAsync(request, cancellationToken);
    }

    public Task UpdateCalendarAsync(string href, string ctag, XDocument body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(ICalDAVClient.Proppatch, href)
        {
            Content = body.ToStringContent()
        }
        .WithVersion(ctag)
        .WithPrefer("return=representation");

        return SendAsync(request, cancellationToken);
    }

    public async Task<ClientResponseCollection<ClientItemResponse<Event>>> GetEventsAsync(string href, XDocument body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(ICalDAVClient.Report, href)
        {
            Content = body.ToStringContent()
        }
        .WithDepth(0)
        .WithPrefer("return-minimal");

        var response = await SendForMultistatusAsync(request, cancellationToken).ConfigureAwait(false);

        ClientResponseCollection<ClientItemResponse<Event>> result = [];

        foreach (var item in response)
        {
            if (item.IsSuccessStatusCode)
            {
                result.Add(new ClientItemResponse<Event>(new Event(item)));
            }
            else
            {
                result.Add(new ClientItemResponse<Event>(ClientResult.Error, ClientError.Failure, $"Failed to retrieve events."));
            }
        }

        return result;
    }

    public async Task<ClientItemResponse<Event>> GetEventAsync(string href, XDocument body,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(ICalDAVClient.Report, href)
        {
            Content = body.ToStringContent()
        }
        .WithDepth(0)
        .WithPrefer("return-minimal");

        var response = await SendForMultistatusAsync(request, cancellationToken).ConfigureAwait(false);

        return (response.FirstOrDefault() is MultistatusItem item && item.IsSuccessStatusCode)
            ? new ClientItemResponse<Event>(new Event(item))
            : new ClientItemResponse<Event>(ClientResult.Error, ClientError.Failure, $"Failed to retrieve event.");
    }

    public Task<ClientItemResponse<Event>> GetEventAsync(string calendarHref, string eventHref,
        CancellationToken cancellationToken = default)
        => GetEventAsync(calendarHref, BodyHelper.CalendarMultiget(eventHref), cancellationToken);

    public Task CreateEventAsync(string href, string body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, href)
        {
            Content = new StringContent(body, Encoding.UTF8, "text/calendar")
        }
        .WithPrefer("return=representation");

        return SendAsync(request, cancellationToken);
    }

    public Task UpdateEventAsync(string href, string etag, string body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, href)
        {
            Content = new StringContent(body, Encoding.UTF8, "text/calendar")
        }
        .WithVersion(etag)
        .WithPrefer("return=representation");

        return SendAsync(request, cancellationToken);
    }

    public Task DeleteAsync(string href, string etag, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, href)
            .WithVersion(etag)
            .WithPrefer("return-minimal");

        return SendAsync(request, cancellationToken);
    }

    public async Task<ChangeCollection> SyncCalendarItemsAsync(string href, string? syncToken, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(ICalDAVClient.Report, href)
        {
            Content = BodyHelper.SyncCollection(syncToken).ToStringContent()
        }
        .WithDepth(1)
        .WithPrefer("return-minimal");

        var response = await SendForMultistatusAsync(request, cancellationToken).ConfigureAwait(false);

        ChangeCollection result = [];

        foreach (var item in response)
        {
            result.Add(new ItemChange(item));
        }

        return result;
    }
}
