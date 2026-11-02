using Markdown.Tokens;

namespace Markdown.Parser.Interfaces;

public interface IMarkdownParser
{
    IEnumerable<Token> Parse(string text);
}
