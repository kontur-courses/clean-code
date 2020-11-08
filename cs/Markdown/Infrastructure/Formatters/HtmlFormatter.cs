using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Tags;

namespace Markdown.Infrastructure.Formatters
{
    public class HtmlFormatter : TagFormatter
    {
        public HtmlFormatter()
        {
            Wrappers = new Dictionary<Style, Func<IEnumerable<string>, IEnumerable<string>>>
            {
                {Style.None, Wrap("", "")},
                {Style.Bold, Wrap("<strong>", "</strong>")},
                {Style.Angled, Wrap("<em>", "</em>")}
            };
        }
        
        public override IEnumerable<string> Format(Style style, IEnumerable<string> words)
        {
            throw new NotImplementedException();
        }
    }
}