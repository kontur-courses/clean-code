using System.Collections;

namespace Markdown;

public class WrapperSettingsProvider : IEnumerable<KeyValuePair<string, TagSetting>>
{
    private readonly Dictionary<string, TagSetting> settings = new();

    public bool TryAddSetting(TagSetting setting)
    {
        return settings.TryAdd(setting.MdTag, setting);
    }

    public bool TryGetSetting(string mdTag, out TagSetting setting)
    {
        return settings.TryGetValue(mdTag, out setting!);
    }

    public bool TryRemoveSetting(string mdTag)
    {
        return settings.Remove(mdTag);
    }

    public IEnumerator<KeyValuePair<string, TagSetting>> GetEnumerator()
    {
        return settings.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}