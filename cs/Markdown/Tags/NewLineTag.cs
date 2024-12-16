namespace Markdown.Tags;

/// <summary>
/// Тег для переноса строки
/// </summary>
public class NewLineTag : ITag
{
    public string MdTag { get; } = "\n";
    public string MdClosingTag { get; } = null;
    public string HtmlTag { get; } = "br";
    public bool SelfClosing { get; } = true;
    public IReadOnlyCollection<ITag> DisallowedChildren { get; } = new List<ITag>();

    public bool Matches(ITag tag)
    {
        return tag is NewLineTag;
    }
}