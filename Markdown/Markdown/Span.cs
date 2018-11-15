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

        public string Assembly(string rowString)
        {
            if (EndIndex - StartIndex == 1)
                return "";

            var builder = new StringBuilder();
            Spans = Spans.OrderBy(s => s.StartIndex).ToList();

            builder.Append(GetOpenTag());
            builder.Append(Spans.Count == 0 ? GetRowSpan(rowString) : GetFullSpan(rowString));
            builder.Append(GetCloseTag());

            return builder.ToString();
        }

        private string GetRowSpan(string rowString)
        {
            return rowString.Substring(StartIndex + TagPair.InitialOpen.Length,
                EndIndex - (StartIndex + TagPair.InitialOpen.Length));
        }

        private string GetFullSpan(string rowString)
        {
            var builder = new StringBuilder();

            builder.Append(rowString.Substring(StartIndex + TagPair.InitialOpen.Length,
                Spans[0].StartIndex - (StartIndex + TagPair.InitialOpen.Length)));

            for (var i = 0; i < Spans.Count - 1; i++)
            {
                builder.Append(Spans[i].Assembly(rowString));
                builder.Append(rowString.Substring(Spans[i].EndIndex + Spans[i].TagPair.InitialClose.Length,
                    Spans[i + 1].StartIndex - (Spans[i].EndIndex + Spans[i].TagPair.InitialClose.Length)));
            }

            var lastSpan = Spans[Spans.Count - 1];
            builder.Append(lastSpan.Assembly(rowString));

            builder.Append(rowString.Substring(lastSpan.EndIndex + lastSpan.TagPair.InitialClose.Length,
                EndIndex - (lastSpan.EndIndex + lastSpan.TagPair.InitialClose.Length)));

            return builder.ToString();
        }

        private string GetOpenTag()
        {
            return !TagPair.CanBeInside && Parent != null && Parent.IsMainSpan == false
                ? TagPair.InitialOpen
                : TagPair.FinalOpen;
        }

        private string GetCloseTag()
        {
            return !TagPair.CanBeInside && Parent != null && Parent.IsMainSpan == false
                ? TagPair.InitialClose
                : TagPair.FinalClose;
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
