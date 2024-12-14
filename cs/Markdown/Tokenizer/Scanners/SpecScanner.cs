using Markdown.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class SpecScanner : ITokenScanner
{
    public Token? Scan(Memory<char> textSlice)
    {
        var tokenValue = textSlice.Span[0];
        var tokenType = GetTokenType(tokenValue);
        
        if (tokenType is null)
        {
            return null;
        }
        var notNullType = (TokenType)tokenType;
        
        return new Token(notNullType, tokenValue.ToString());
    }

    public static bool CanScan(char symbol) => GetTokenType(symbol) != null;
    
    private static TokenType? GetTokenType(char symbol) => symbol switch
    {
        ' ' or '\u00a0' or '\u200b' => TokenType.Space,
        '*' => TokenType.Asterisk,
        '\n' or '\r' => TokenType.Newline,
        '\\' => TokenType.Backslash,
        '_' => TokenType.Underscore,
        '#' => TokenType.Octothorpe,
        _ => null
    };
}