using Markdown.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class SpecScanner : ITokenScanner
{
    public Token? Scan(string text, int begin = 0)
    {
        var tokenType = GetTokenType(text[begin]);
        
        if (tokenType is null)
        {
            return null;
        }
        var notNullType = (TokenType)tokenType;
        
        return new Token(notNullType, begin, 1, text);
    }

    public static bool CanScan(char symbol) => GetTokenType(symbol) != null;
    
    private static TokenType? GetTokenType(char symbol) => symbol switch
    {
        ' ' => TokenType.Space,
        '*' => TokenType.Asterisk,
        '\n' or '\r' => TokenType.Newline,
        '\\' => TokenType.Backslash,
        '_' => TokenType.Underscore,
        '#' => TokenType.Octothorpe,
        _ => null
    };
}