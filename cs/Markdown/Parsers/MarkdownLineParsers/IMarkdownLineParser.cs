using Markdown.Tokens;

namespace Markdown.Parsers.MarkdownLineParsers;

public interface IMarkdownLineParser
{
    IEnumerable<IToken> ParseParagraphForTokens(string markdownLineParagraph);

    IEnumerable<string> ParseMarkdownTextIntoParagraphs(string markdownLine);
}