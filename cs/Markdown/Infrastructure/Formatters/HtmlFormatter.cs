using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Formatters
{
    public class HtmlFormatter : BlockFormatter
    {
        public HtmlFormatter()
        {
            Wrappers = new Dictionary<Style, Func<IEnumerable<string>, IEnumerable<string>>>
            {
                {Style.None, Wrap("", "")},
                {Style.Bold, Wrap("<strong>", "</strong>")},
                {Style.Angled, Wrap("<em>", "</em>")},
                {Style.Header, Wrap("<h1>", "</h1>")},
            };
        }
        
        public override IEnumerable<string> Format(Style style, IEnumerable<string> words)
        {
            return Wrappers.TryGetValue(style, out var wrap) 
                ? wrap(words) 
                : words;
        }
    }
}