namespace Markdown;

public class TagConfigurationList 
{
    private List<TagConfiguration> _tagsConfigurations = new List<TagConfiguration>()
    {
        new TagConfiguration("_", TagNames.ItalicOpen, TagNames.ItalicClose, 2), 
        new TagConfiguration("__", TagNames.BoldOpen, TagNames.BoldClose, 1), 
        new TagConfiguration("#", TagNames.TitleOpen, TagNames.TitleClose, 1), 
        new TagConfiguration("$", TagNames.LinkOpen, TagNames.LinkClose, 1), 
    };
    
    public List<TagConfiguration> TagConfigurations
    {
        get => _tagsConfigurations;
    }
}