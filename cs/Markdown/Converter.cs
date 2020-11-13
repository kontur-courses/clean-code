using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Converter
    {
        private static Dictionary<TagType, HtmlTag> _tagTypeToHtml = new Dictionary<TagType, HtmlTag>()
        {
            {TagType.None, new HtmlTag("", "")},
            {TagType.Bold, new HtmlTag("<strong>", "</strong>")},
            {TagType.Heading, new HtmlTag("<h1>", "</h1>")},
            {TagType.Italics, new HtmlTag("<em>", "</em>")}
        };
        public static string ToHTML(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private class HtmlTag
        {
            public string Start { get; }
            public string End { get; }

            public HtmlTag(string start, string end)
            {
                Start = start;
                End = end;
            }
        }
    }
}
