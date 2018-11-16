using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Span
    {
        public Tag Tag { get; }
        public int StartIndex { get; }
        public int EndIndex { get; private set; }

        public List<Span> Spans { get; private set; }
        public Span Parent { get; private set; }
        public bool IsClosed { get; private set; }

        public Span(Tag tag, int startIndex)
        {
            Tag = tag;
            StartIndex = startIndex;
            Spans = new List<Span>();
        }

        public Span(Tag tag, int startIndex, int endIndex, bool isClosed=true)
        {
            Tag = tag;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Spans = new List<Span>();
            IsClosed = isClosed;
        }

        public void Close(int endIndex)
        {
            EndIndex = endIndex;
            IsClosed = true;

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
                if (span.IsClosed)
                    continue;

                Spans.Remove(span);
                foreach (var childSpan in span.Spans)
                {
                    Spans.Add(childSpan);
                    childSpan.Parent = this;
                }
            }
            Spans = Spans.OrderBy(s => s.StartIndex).ToList();
        }
    }
}
