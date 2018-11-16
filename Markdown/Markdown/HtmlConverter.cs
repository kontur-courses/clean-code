using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        public string Convert(string rawString, Span span)
        {
            return Assembly(rawString, span);
        }

        private string Assembly(string rawString, Span span)
        {
            if (span.EndIndex - span.StartIndex == 1 && rawString[span.StartIndex] == '\\')
                return "";

            var builder = new StringBuilder();

            builder.Append(GetOpenTag(span));
            builder.Append(span.Spans.Count == 0 ? GetSpanRowString(rawString, span) : GetSpanAssembledString(rawString, span));
            builder.Append(GetCloseTag(span));

            return builder.ToString();
        }

        private static string GetOpenTag(Span initialSpan)
        {
            var tagOpen = Markups.Html.Tags.FirstOrDefault(t => t.Value == initialSpan.Tag.Value);
            if (tagOpen == null)
                return initialSpan.Tag.Open;

            return initialSpan.Parent?.Parent != null && initialSpan.Tag.CantBeInside.Contains(initialSpan.Parent.Tag.Value)
                ? initialSpan.Tag.Open
                : tagOpen.Open;
        }

        private static string GetCloseTag(Span initialSpan)
        {
            var tagClose = Markups.Html.Tags.FirstOrDefault(t => t.Value == initialSpan.Tag.Value);
            if (tagClose == null)
                return initialSpan.Tag.Close;

            return initialSpan.Parent?.Parent != null && initialSpan.Tag.CantBeInside.Contains(initialSpan.Parent.Tag.Value)
                ? initialSpan.Tag.Close
                : tagClose.Close;
        }

        private static string GetSpanRowString(string rawString, Span span)
        {
            return rawString.Substring(span.StartIndex + span.Tag.Open.Length,
                span.EndIndex - (span.StartIndex + span.Tag.Open.Length));
        }

        private string GetSpanAssembledString(string rawString, Span span)
        {
            var builder = new StringBuilder();

            builder.Append(rawString.Substring(span.StartIndex + span.Tag.Open.Length,
                span.Spans[0].StartIndex - (span.StartIndex + span.Tag.Open.Length)));

            for (var i = 0; i < span.Spans.Count - 1; i++)
            {
                builder.Append(Assembly(rawString, span.Spans[i]));
                builder.Append(rawString.Substring(span.Spans[i].EndIndex + span.Spans[i].Tag.Close.Length,
                    span.Spans[i + 1].StartIndex - (span.Spans[i].EndIndex + span.Spans[i].Tag.Close.Length)));
            }

            var lastSpan = span.Spans[span.Spans.Count - 1];
            builder.Append(Assembly(rawString, lastSpan));

            builder.Append(rawString.Substring(lastSpan.EndIndex + lastSpan.Tag.Close.Length,
                span.EndIndex - (lastSpan.EndIndex + lastSpan.Tag.Close.Length)));

            return builder.ToString();
        }
    }
}
