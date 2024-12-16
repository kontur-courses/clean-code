using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class WordScanner : ITokenScanner
{
    public Token? Scan(Memory<char> textSlice)
    {
        var valueLen = 0;
        var textSpan = textSlice.Span;
        
        while (valueLen < textSpan.Length && CanScan(textSpan[valueLen])) valueLen++;
        return valueLen == 0 ? null : new Token(TokenType.WORD, textSlice[..valueLen].ToString());
    }

    private static bool CanScan(char symbol)
        => !SpecScanner.CanScan(symbol) && !NumberScanner.CanScan(symbol);
}