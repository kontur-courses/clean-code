using Markdown.Parsers;

namespace Markdown
{
    public interface ITokenizer
    {
        (Token[] tokens, string text) Tokenize(string text);
    }
}
