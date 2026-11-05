namespace Markdown.Tokens;

public class LinkToken : Token
{
    public LinkToken(string raw, int start, string text, string url, List<Token> content)
        : base(TokenKind.Link, raw, start)
    {
        Text = text;
        Url = url;
        Content = content;
    }
    
    public string Text { get; }

    public string Url { get; }
    
    public List<Token> Content { get; }
}