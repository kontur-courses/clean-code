using System.Linq;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class NumberScanner : ITokenScanner
{
    public bool TryGetToken(string plainMarkdown, int index, out Token? token)
    {
        token = default;
        
        var number = new string(plainMarkdown
            .Skip(index)
            .TakeWhile(char.IsDigit)
            .ToArray());

        if (number.Length == 0)
        {
            return false;
        }
        
        token = new Token(TypeOfToken.Number, number);
        return true;
    }
}