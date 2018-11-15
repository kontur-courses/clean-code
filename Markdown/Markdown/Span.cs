using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Span
    {
        public TagPair TagPair { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public List<Span> Spans { get; set; }
        public Span Parent { get; set; }
        public bool IsMainSpan = false;

        public Span(TagPair tagPair, int startIndex)
        {
            TagPair = tagPair;
            StartIndex = startIndex;
            Spans = new List<Span>();
        }

        public Span(TagPair tagPair, int startIndex, int endIndex)
        {
            TagPair = tagPair;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Spans = new List<Span>();
        }

        public void PutSpan(Span span)
        {
            var nextSpan = Spans
                .OrderByDescending(s => s.StartIndex)
                .FirstOrDefault(s => s.StartIndex < span.StartIndex &&
                            (s.EndIndex > span.StartIndex || s.EndIndex == 0));

            if (nextSpan == null)
            {
                Spans.Add(span);
                span.Parent = this;
                return;
            }

            nextSpan.PutSpan(span);
        }

        public void RemoveNotClosedSpans()
        {
            foreach (var span in Spans.ToArray())
            {
                span.RemoveNotClosedSpans();
                if (span.EndIndex != 0)
                    continue;

                Spans.Remove(span);
                foreach (var spanSpan in span.Spans)
                {
                    Spans.Add(spanSpan);
                    spanSpan.Parent = this;
                }
            }
        }
    }
}
