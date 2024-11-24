using Markdown.Tokens;

namespace Markdown.Scanners;

public class TextScanner : ITokenScanner
{
    private readonly SpecScanner specScanner = new();

    public Token? Scan(string text)
    {
        var textValue = text
            .Select(c => c.ToString())
            .TakeWhile(c => specScanner.Scan(c) != null)
            .ToString();
        return textValue == null ? null : new Token(TokenType.Text, textValue);
    }
}