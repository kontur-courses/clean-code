namespace Markdown.Tokens.CommonTokens;

public class DigitToken : CommonToken
{
    public DigitToken(string content)
    {
        Content = content;
    }

    public override TokenType Type => TokenType.Digit;

    public override string Content { get; }
}