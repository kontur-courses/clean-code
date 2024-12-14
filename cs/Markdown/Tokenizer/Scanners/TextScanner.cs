using Markdown.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class TextScanner : ITokenScanner
{
    public Token? Scan(Memory<char> textSlice)
    {
        var valueLength = 0;
        var textSpan = textSlice.Span;
        
        while (valueLength < textSpan.Length && CanScan(textSpan[valueLength]))
        {
            valueLength++;
        }
        return valueLength == 0 ? null : new Token(TokenType.Word, textSlice[..valueLength].ToString());
    }

    private static bool CanScan(char symbol) 
        => !SpecScanner.CanScan(symbol) && !NumberScanner.CanScan(symbol);

}