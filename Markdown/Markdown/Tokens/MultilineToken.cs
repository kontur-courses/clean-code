namespace Markdown.Tokens;

public class MultilineToken : Token
{
    public MultilineToken(string opening, string ending, TokenType type)
        : base(opening, ending, type) { }
}