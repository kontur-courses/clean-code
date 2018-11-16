using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        public string Convert(string rowString, Span span)
        {
            return Assembly(rowString, span);
        }

        public string Assembly(string rowString, Span span)
        {
            if (span.EndIndex - span.StartIndex == 1)
                return "";

            var builder = new StringBuilder();

            builder.Append(GetOpenTag(span));
            builder.Append(span.Spans.Count == 0 ? GetRowSpan(rowString, span) : GetFullSpan(rowString, span));
            builder.Append(GetCloseTag(span));

            return builder.ToString();
        }

        private string GetOpenTag(Span span)
        {
            var tagOpen = Markups.Html.Tags.FirstOrDefault(t => t.Value == span.Tag.Value);
            if (tagOpen == null)
                return span.Tag.Open;
            return !span.Tag.CanBeInside && span.Parent?.Parent != null
                ? span.Tag.Open
                : tagOpen.Open;
        }

        private string GetCloseTag(Span span)
        {
            var tagClose = Markups.Html.Tags.FirstOrDefault(t => t.Value == span.Tag.Value);
            if (tagClose == null)
                return span.Tag.Close;

            return !span.Tag.CanBeInside && span.Parent?.Parent != null
                ? span.Tag.Close
                : tagClose.Close;
        }

        private string GetRowSpan(string rowString, Span span)
        {
            return rowString.Substring(span.StartIndex + span.Tag.Open.Length,
                span.EndIndex - (span.StartIndex + span.Tag.Open.Length));
        }

        private string GetFullSpan(string rowString, Span span)
        {
            var builder = new StringBuilder();

            builder.Append(rowString.Substring(span.StartIndex + span.Tag.Open.Length,
                span.Spans[0].StartIndex - (span.StartIndex + span.Tag.Open.Length)));

            for (var i = 0; i < span.Spans.Count - 1; i++)
            {
                builder.Append(Assembly(rowString, span.Spans[i]));
                builder.Append(rowString.Substring(span.Spans[i].EndIndex + span.Spans[i].Tag.Close.Length,
                    span.Spans[i + 1].StartIndex - (span.Spans[i].EndIndex + span.Spans[i].Tag.Close.Length)));
            }

            var lastSpan = span.Spans[span.Spans.Count - 1];
            builder.Append(Assembly(rowString, lastSpan));

            builder.Append(rowString.Substring(lastSpan.EndIndex + lastSpan.Tag.Close.Length,
                span.EndIndex - (lastSpan.EndIndex + lastSpan.Tag.Close.Length)));

            return builder.ToString();
        }
    }
}
