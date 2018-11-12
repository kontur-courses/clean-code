using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Span
    {
        public Tag Tag;
        public int StartIndex;
        public int EndIndex;
        public List<Span> Spans;

        public Span(Tag tag, int startIndex)
        {
            Tag = tag;
            StartIndex = startIndex;
            Spans = new List<Span>();
        }

        public Span(Tag tag, int startIndex, int endIndex)
        {
            Tag = tag;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Spans = new List<Span>();
        }

        public string Assembly(string rowString)
        {
            if (EndIndex - StartIndex == 1)
                return "";
            var builder = new StringBuilder();
            
            builder.Append(Tag.HtmlStart);

            if (Spans.Count == 0)
            {
                builder.Append(rowString.Substring(StartIndex + Tag.MarkdownStart.Length,
                    EndIndex - StartIndex - Tag.MarkdownEnd.Length)); 
            }
            else
            {
                builder.Append(rowString.Substring(StartIndex + Tag.MarkdownStart.Length,
                            Spans[0].StartIndex - (StartIndex + Tag.MarkdownStart.Length)));

                for (var i = 0; i < Spans.Count -1; i++)
                {
                    builder.Append(Spans[i].Assembly(rowString));
                    builder.Append(rowString.Substring(Spans[i].EndIndex + Spans[i].Tag.MarkdownEnd.Length,
                        Spans[i + 1].StartIndex - (Spans[i].EndIndex + Spans[i].Tag.MarkdownEnd.Length)));
                }

                var lastSpan = Spans[Spans.Count - 1];
                builder.Append(lastSpan.Assembly(rowString));

                builder.Append(rowString.Substring(lastSpan.EndIndex  + lastSpan.Tag.MarkdownEnd.Length,
                    EndIndex - (lastSpan.EndIndex + lastSpan.Tag.MarkdownEnd.Length - 1)));
            }
            builder.Append(Tag.HtmlEnd);

            return builder.ToString();
        }
    }
}
