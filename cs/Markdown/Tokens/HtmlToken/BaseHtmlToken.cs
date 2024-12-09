namespace Markdown.Tokens.HtmlToken;

public abstract class BaseHtmlToken
{
    public List<BaseHtmlToken> Children { get; }
    public abstract string Value { get; }

    public BaseHtmlToken(List<BaseHtmlToken>? children)
    {
        Children = children;
    }
}