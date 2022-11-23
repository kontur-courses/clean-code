using Markdown.Token;

namespace Markdown.Parse;

public interface IMarkupParser
{
    public TokenTree Parse(string text);
}