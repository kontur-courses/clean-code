using Markdown.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class TextScanner : ITokenScanner
{
    public Token? Scan(string text, int begin = 0)
    {
        var valueEnumerable = text
            .Skip(begin)
            .TakeWhile(CanScan);
        var valueLen = valueEnumerable.Count();
        return valueLen == 0 ? null : new Token(TokenType.Word, begin, valueLen, text);
    }

    private static bool CanScan(char symbol) 
        => !SpecScanner.CanScan(symbol) && !NumberScanner.CanScan(symbol);

}