namespace Markdown;

public static class TagConfigurations
{
    public static readonly Dictionary<string, TagConfiguration> TagsConfigurations = new Dictionary<string, TagConfiguration>()
    {
        { "_", new TagConfiguration("_", TagNames.ItalicOpen, TagNames.ItalicClose) },
        { "__", new TagConfiguration("__", TagNames.BoldOpen, TagNames.BoldClose) }, 
        { "#", new TagConfiguration("#", TagNames.TitleOpen, TagNames.TitleClose) }, 
    };
}