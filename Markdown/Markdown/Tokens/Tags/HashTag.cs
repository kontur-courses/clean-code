namespace Markdown.Tokens.Tags;

public class HashTag : ITag
{
    public ETagType Type => ETagType.Header;
    public string HtmlTag => "h1";
}