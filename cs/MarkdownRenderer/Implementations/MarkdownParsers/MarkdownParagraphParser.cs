using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownParagraphParser : MarkdownDefaultElementParser<ParagraphElement>
{
    public override ElementParseType ParseType => ElementParseType.Line;

    public override ParagraphElement ParseElement(string content, Token contentToken)
    {
        return new ParagraphElement(content.Substring(contentToken));
    }
}