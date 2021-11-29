using System.Collections.Generic;

namespace Markdown.Parsers
{
    public interface IMarkdownParser
    {
        public IEnumerable<string> ParseMarkdownLexemes(string markdown);
    }
}