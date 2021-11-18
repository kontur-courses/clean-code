using Markdown.Markings;

namespace Markdown.Parsers
{
    public interface IMarkdownParser
    {
        public IMarkdownMarking Parse(string markdown);
    }
}