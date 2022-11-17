using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownPlainTextParser : MarkdownDefaultElementParser<PlainText>
{
    public override ElementParseType ParseType => ElementParseType.Inline;

    public override PlainText ParseElement(string content, Token contentToken)
    {
        return new PlainText(content.Substring(contentToken));
    }
}