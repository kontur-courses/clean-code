namespace Markdown.Tokens;

public class LinkToken : BaseToken
{
    public string Url { get; }

    public LinkToken(IEnumerable<BaseToken> labelTokens, string url)
        : base(TokenType.Link)
    {
        Url = url;
        Children.AddRange(labelTokens);
    }
}