using System.Collections;

namespace CalDAVNet;

public class ChangeCollection : IEnumerable<ItemChange>
{
    private readonly List<ItemChange> _changes = [];

    public string? SyncToken { get; init; }

    public int Count => _changes.Count;

    public ItemChange this[int index] => _changes[index];

    internal ChangeCollection()
    {
    }

    internal void Add(ItemChange change)
    {
        _changes.Add(change);
    }

    public IEnumerator<ItemChange> GetEnumerator()
    {
        return _changes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
