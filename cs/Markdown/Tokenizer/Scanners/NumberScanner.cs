using Markdown.Tokens;

namespace Markdown.Tokenizer.Scanners;

public class NumberScanner : ITokenScanner
{
    public Token? Scan(string text, int begin = 0)
    {
        var numberEnumerable = text
            .Skip(begin)
            .TakeWhile(CanScan);
        var numberLength = numberEnumerable.Count();
        return numberLength == 0 ? null : new Token(TokenType.Number, begin, numberLength, text);
    }

    public static bool CanScan(char symbol) => char.IsDigit(symbol);
}