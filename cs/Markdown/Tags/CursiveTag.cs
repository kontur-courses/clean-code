namespace Markdown.Tags;

/// <summary>
/// Тег для курсивного текста
/// </summary>
public class CursiveTag : ITag
{
    public string MdTag { get; } = "_";

    public string MdClosingTag => MdTag;

    public string HtmlTag { get; } = "em";

    public IReadOnlyCollection<ITag> DisallowedChildren { get; } = new List<ITag> { new StrongTag() };

    public bool Matches(ITag tag)
    {
        return tag is CursiveTag;
    }
}