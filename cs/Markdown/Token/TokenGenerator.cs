namespace Markdown;

public class TokenGenerator : ITokenGenerator
{
    private readonly List<ITokenRule> rules =
    [
        new LinkRule(),
        new NewLineRule(),
        new WhiteSpaceRule(),
        new EscapeRule(),
        new HeaderRule(),
        new UnderscoreRule(),
        new TextRunRule(),
        new DigitRule()
    ];

    public List<Token> Tokenize(string text)
    {
        if (string.IsNullOrEmpty(text))
            return null;
        var tokens = new List<Token>();

        var cursor = new TextCursor(text);
        while (!cursor.End)
        {
            var token = TryCreateToken(cursor);
            tokens.Add(token);
        }
        tokens.Add(TokenFactory.Create(TokenType.EndOfText, null));
        return tokens;
    }

    private Token? TryCreateToken(TextCursor cursor)
    {
        Token? token = null;
        foreach (var rule in rules)
        {
            token = rule.TryReadTokenAndMoveCursor(cursor);
            if (token != null)
                return token;
        }

        return token;
    }
}