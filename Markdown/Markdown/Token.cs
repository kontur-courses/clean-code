using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public Tag Tag;
        public int StartIndex;
        public int EndIndex;

        public Token(Tag tag, int startIndex)
        {
            Tag = tag;
            StartIndex = startIndex;
        }

        public string Assembly(string rowString)
        {
            var builder = new StringBuilder();
            builder.Append(Tag.HtmlStart);
            builder.Append(rowString.Substring(StartIndex + Tag.MarkdownStart.Length, EndIndex - StartIndex - Tag.MarkdownEnd.Length));
            builder.Append(Tag.HtmlEnd);
            return builder.ToString();
        }
    }
}
