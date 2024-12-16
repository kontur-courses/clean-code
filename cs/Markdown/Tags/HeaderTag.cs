namespace Markdown.Tags;

/// <summary>
/// Тег для заголовка. Может быть использован только в начале строки
/// </summary>
public class HeaderTag : ITag
{
    public string MdTag { get; } = "#";
    public string MdClosingTag { get; } = "\n";
    public string HtmlTag { get; } = "h1";
    public IReadOnlyCollection<ITag> DisallowedChildren { get; } = new List<ITag>();

    public bool Matches(ITag tag)
    {
        return tag is HeaderTag;
    }
}