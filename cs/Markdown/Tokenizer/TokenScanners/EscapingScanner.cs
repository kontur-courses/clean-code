using System.Linq;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class EscapingScanner : ITokenScanner
{
    public bool TryGetToken(string plainMarkdown, int index, out Token? token)
    {
        var escapedCharacters = new[] { '\\', '_', '#' };
        token = default;

        if (plainMarkdown.Length <= index + 1 || plainMarkdown[index] != '\\' || !escapedCharacters.Contains(plainMarkdown[index + 1]))
        {
            return false;
        }
        
        token = new Token(TypeOfToken.Word, plainMarkdown[index+1].ToString(), 2);
        return true;
    }
}