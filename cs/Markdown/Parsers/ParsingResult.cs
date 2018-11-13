using Markdown.Elements;

namespace Markdown.Parsers
{
    public class ParsingResult
    {
        public readonly bool Success;
        public readonly MarkdownElement Element;

        public ParsingResult(bool success, MarkdownElement element)
        {
            Success = success;
            Element = element;
        }
    }
}
