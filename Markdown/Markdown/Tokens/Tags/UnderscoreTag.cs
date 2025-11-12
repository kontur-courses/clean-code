namespace Markdown.Tokens.Tags;

public class UnderscoreTag : ITag
{
    public ETagType Type => ETagType.Italics;
    public string HtmlTag => "em";
}