namespace Markdown.Tokens.Tags;

public class TextTag : ITag
{
    public ETagType Type => ETagType.Text;
    public string HtmlTag { get; }
}