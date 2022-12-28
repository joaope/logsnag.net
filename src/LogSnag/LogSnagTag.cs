using System.Text.RegularExpressions;

namespace LogSnag;

public readonly struct LogSnagTag
{
    private readonly int? _intValue;
    private readonly string? _stringValue;
    private readonly bool? _boolValue;
    private static readonly Regex KeyValidation = new("[a-z]+(-?[a-z]+)$", RegexOptions.Singleline | RegexOptions.Compiled);

    public string Key { get; }
    public object? Value => _intValue ?? (object?)_boolValue ?? _stringValue;

    public LogSnagTag(string key, int value)
    {
        Key = AssertAndReturnKey(key);

        _intValue = value;
        _stringValue = null;
        _boolValue = null;
    }

    public LogSnagTag(string key, string value)
    {
        Key = AssertAndReturnKey(key);

        _intValue = null;
        _stringValue = value;
        _boolValue = null;
    }

    public LogSnagTag(string key, bool value)
    {
        Key = AssertAndReturnKey(key);

        _stringValue = null;
        _intValue = null;
        _boolValue = value;
    }

    private static string AssertAndReturnKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Tag key cannot be null or empty.", nameof(key));
        }

        if (!KeyValidation.IsMatch(key))
        {
            throw new ArgumentException(
                "Tag key may only consist of lowercase alphabet characters and optionally separated with dashes (-)")
            {
                Data =
                {
                    {"key", key}
                }
            };
        }

        return key;
    }
}