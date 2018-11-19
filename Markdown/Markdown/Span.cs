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

        private List<Span> children = new List<Span>();
        public List<Span> Children => children;
        public Span Parent { get; private set; }
        public bool IsClosed => EndIndex != 0;
        public bool CanBeInside => Tag.Type == TagType.None || (Parent != null && !Tag.CanBeInside.Contains(Parent.Tag.Type));
        public int IndexAfterStart => StartIndex + Tag.Open.Length;
        public int IndexAfterEnd => EndIndex + Tag.Close.Length;

        public Span(Tag tag, int startIndex, int endIndex=0)
        {
            Tag = tag;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public void Close(int endIndex)
        {
            if (!IsClosed)
                EndIndex = endIndex;

        }

        public void PutSpan(Span span)
        {
            var nextSpan = Children
                .OrderByDescending(s => s.StartIndex)
                .FirstOrDefault(s => s.StartIndex < span.StartIndex &&
                            (s.EndIndex > span.StartIndex || !s.IsClosed));

            if (nextSpan == null)
            {
                children.Add(span);
                span.Parent = this;
                return;
            }

            nextSpan.PutSpan(span);
        }

        public void RemoveNotClosedSpans()
        {
            foreach (var span in children.ToArray())
            {
                span.RemoveNotClosedSpans();
                if (span.IsClosed)
                    continue;

                children.Remove(span);
                foreach (var childSpan in span.children)
                {
                    children.Add(childSpan);
                    childSpan.Parent = this;
                }
            }
            children = children.OrderBy(s => s.StartIndex).ToList();
        }
    }
}
