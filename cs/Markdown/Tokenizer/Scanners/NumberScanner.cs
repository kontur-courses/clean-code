using System.Collections;
using Markdown.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class NumberScanner : ITokenScanner
{
    public Token? Scan(Memory<char> textSlice)
    {
        var numberLength = 0;
        var textSpan = textSlice.Span;
        
        while (numberLength < textSpan.Length && CanScan(textSpan[numberLength]))
        {
            numberLength++;
        }
        
        return numberLength == 0 ? null : new Token(TokenType.Number, textSlice[..numberLength].ToString());
    }

    public static bool CanScan(char symbol) => char.IsDigit(symbol);
}