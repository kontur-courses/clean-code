namespace Markdown.Tokens.Tags;

public class DoubleUnderscoreTag : ITag
{
    public ETagType Type => ETagType.Bold;
    public string HtmlTag => "strong";
}