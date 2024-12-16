using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class NumberScanner : ITokenScanner
{
    public Token? Scan(Memory<char> textSlice)
    {
        var numberLen = 0;
        var textSpan = textSlice.Span;
        
        while (numberLen < textSpan.Length && CanScan(textSpan[numberLen])) numberLen++;
        return numberLen == 0 ? null : new Token(TokenType.NUMBER, textSlice[..numberLen].ToString());
    }
    
    public static bool CanScan(char symbol) => char.IsDigit(symbol);
}