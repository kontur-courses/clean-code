using Markdown.Tokens;

namespace Markdown.Scanners;

public interface ITokenScanner
{
    public Token? Scan(string text);
}