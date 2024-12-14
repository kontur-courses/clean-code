namespace Markdown.Tokens.CommonTokens;

public class TextToken : CommonToken
{
    public TextToken(string content)
    {
        Content = content;
    }

    public override TokenType Type => TokenType.Text;
    public override string Content { get; }
}