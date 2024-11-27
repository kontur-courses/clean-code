using Markdown.Tokens;

namespace Markdown.Tokenizer.Scanners;

public interface ITokenScanner
{
    public Token? Scan(string text, int begin);
}