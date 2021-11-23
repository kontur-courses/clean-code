using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parsers
{
    public interface IMarkdownParser
    {
        public IEnumerable<string> Parse(string markdown);
    }
}