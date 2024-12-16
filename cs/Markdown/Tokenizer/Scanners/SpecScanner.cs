using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class SpecScanner : ITokenScanner
{
    public Token? Scan(Memory<char> textSlice)
    {
        var tokenType = GetTokenType(textSlice.Span[0]);
        if (tokenType is null) return null;
        
        var notNullType = (TokenType)tokenType;
        return new Token(notNullType, textSlice.Span[0].ToString());
    }
    
    public static bool CanScan(char symbol) 
        => GetTokenType(symbol) != null;

    private static TokenType? GetTokenType(char symbol) => symbol switch
    {
        ' ' => TokenType.SPACE,
        '#' => TokenType.HASH_TAG,
        '_' => TokenType.UNDERSCORE,
        '\\' => TokenType.BACK_SLASH,
        '\n' or '\r' => TokenType.NEW_LINE,
        
        '(' => TokenType.OPEN_BRACKET,
        ')' => TokenType.CLOSE_BRACKET,
        '[' => TokenType.OPEN_SQUARE_BRACKET,
        ']' => TokenType.CLOSE_SQUARE_BRACKET,
        
        _ => null
    };
}