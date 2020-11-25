using System;
using Markdown.MarkdownProcessors;
using Markdown.Parsers;

namespace Markdown
{
    public class ItalicMark : Mark
    {
        public ItalicMark()
        {
            DefiningSymbol = "_";
            AllSymbols = new[] {"_", "_"};
            FormattedMarkSymbols = ("\\<em>", "\\</em>");
        }
    }
}