using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class NewlineScanner : ITokenScanner
{
    public bool TryGetToken(string plainMarkdown, int index, out Token? token)
    {
        token = default;
        
        if (plainMarkdown[index] == '\r' && plainMarkdown.Length > index + 1 && plainMarkdown[index + 1] == '\n')
        {
            token = new Token(TypeOfToken.Newline, "\r\n");
            return true;
        }
        
        if (plainMarkdown[index] == '\n')
        {
            token = new Token(TypeOfToken.Newline, "\n");
            return true;
        }

        return false;
    }
}