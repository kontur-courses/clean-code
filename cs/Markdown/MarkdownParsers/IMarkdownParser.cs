using Markdown.TokenInfo;

namespace Markdown.MarkdownParsers;

public interface IMarkdownParser
{
    IEnumerable<Token> Parse(string markdown);
}