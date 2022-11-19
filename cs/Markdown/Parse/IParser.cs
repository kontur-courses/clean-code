using Markdown.Token;

namespace Markdown.Parser;

public interface IParser
{
    public TokenTree Parse(string text);
}