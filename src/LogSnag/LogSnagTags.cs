using System.Collections;

namespace LogSnag;

public sealed class LogSnagTags : IList<LogSnagTag>
{
    private readonly IList<LogSnagTag> _listImplementation = new List<LogSnagTag>();

    public IEnumerator<LogSnagTag> GetEnumerator() => _listImplementation.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_listImplementation).GetEnumerator();

    public void Add(LogSnagTag item) => _listImplementation.Add(item);

    public void Clear() => _listImplementation.Clear();

    public bool Contains(LogSnagTag item) => _listImplementation.Contains(item);

    public void CopyTo(LogSnagTag[] array, int arrayIndex) => _listImplementation.CopyTo(array, arrayIndex);

    public bool Remove(LogSnagTag item) => _listImplementation.Remove(item);

    public int Count => _listImplementation.Count;

    public bool IsReadOnly => _listImplementation.IsReadOnly;

    public int IndexOf(LogSnagTag item) => _listImplementation.IndexOf(item);

    public void Insert(int index, LogSnagTag item) => _listImplementation.Insert(index, item);

    public void RemoveAt(int index) => _listImplementation.RemoveAt(index);

    public LogSnagTag this[int index]
    {
        get => _listImplementation[index];
        set => _listImplementation[index] = value;
    }
}