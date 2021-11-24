using System.Collections;

namespace Markdown;

public class WrapperSettingsProvider : IEnumerable<TagSetting>
{
    private readonly Dictionary<string, TagSetting> settings = new();

    public WrapperSettingsProvider(params TagSetting[] settings) : this((IEnumerable<TagSetting>)settings)
    { }

    public WrapperSettingsProvider(IEnumerable<TagSetting> settings)
    {
        foreach (var setting in settings)
        {
            if (!TryAddSetting(setting))
            {
                throw new ArgumentException($"Failed to add setting with key: {setting.OpeningTag}");
            }
        }
    }

    public bool TryAddSetting(TagSetting setting)
    {
        if (setting?.OpeningTag == null)
            throw new ArgumentNullException(nameof(setting));
        return settings.TryAdd(setting.OpeningTag, setting);
    }

    public bool TryGetSetting(string mdTag, out TagSetting setting)
    {
        return settings.TryGetValue(mdTag, out setting!);
    }

    public bool TryRemoveSetting(string mdTag)
    {
        return settings.Remove(mdTag);
    }

    public TagSetting[] GetSettings(bool isLineOnly)
    {
        return settings.Values
            .Where(x => x.IsLineOnly == isLineOnly)
            .OrderByDescending(x => x.OpeningTag.Length)
            .ToArray();
    }

    public IEnumerator<TagSetting> GetEnumerator()
    {
        return settings.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}