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
            if (span.IsIgnored)
                return "";

            var builder = new StringBuilder();

            builder.Append(GetTagValue(span, t => t.Open));

            if (span.Children.Count == 0)
                builder.Append(rawString.Segment(span.IndexAfterStart, span.EndIndex));

            foreach (var child in span.Children)
                builder.Append(Assembly(rawString, child));

            builder.Append(GetTagValue(span, t => t.Close));

            return builder.ToString();
        }

        private static string GetTagValue(Span initialSpan, Func<Tag, string> getTag)
        {
            var tag = Markups.Html.Tags.FirstOrDefault(t => t.Type == initialSpan.Tag.Type);
            if (tag == null)
                return getTag(initialSpan.Tag);

            return initialSpan.CanBeInside
                ? getTag(initialSpan.Tag)
                : getTag(tag);
        }
    }
}
