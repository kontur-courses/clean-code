using Markdown.Tokens;

namespace Markdown.Interfaces;

public interface IMarkdownParser
{
    IEnumerable<BaseToken> ParseTokens(string markdownText);
}