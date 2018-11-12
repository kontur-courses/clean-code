using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        private readonly List<Tag> tags;
        private int index;
        private readonly Tag emptyTag = new Tag("", "", "", "");

        public Md()
        {
            tags = new List<Tag>
            {
                new Tag("_", "_", "<em>", @"<\em>"),
                new Tag("__", "__", "<strong>", @"<\strong>")
            };
        }

        public string Render(string markdownString)
        {
            index = 0;
            return ParseWithoutRegexp(markdownString);
        }

        private string ParseWithoutRegexp(string markdownString)
        {
            var mainSpan = new Span(emptyTag, 0, markdownString.Length);
            var openedSpans = new List<Span>();

            for (; index < markdownString.Length; index++)
            {
                if (markdownString[index] == '\\')
                {
                    PutSpanInSpan(new Span(emptyTag, index, index + 1), mainSpan);
                    index += 1;
                    continue;
                }

                if (openedSpans.Count != 0)
                {
                    var span = FindSpanEnd(markdownString, openedSpans);
                    if (span != null)
                    {
                        openedSpans = openedSpans.Where(t => t.EndIndex == 0).ToList();
                        continue;
                    }
                }

                var newSpan = FindSpanStart(markdownString);
                if (newSpan != null)
                {
                    PutSpanInSpan(newSpan, mainSpan);
                    openedSpans.Add(newSpan);
                }
            }
            
            return mainSpan.Assembly(markdownString);
        }

        private void PutSpanInSpan(Span child, Span parent)
        {
            while (true)
            {
                if (parent.Spans.Count == 0)
                {
                    parent.Spans.Add(child);
                    break;
                }

                var nextSpan = parent.Spans
                    .OrderByDescending(s => s.StartIndex)
                    .FirstOrDefault(s => s.StartIndex < child.StartIndex &&
                                        (s.EndIndex > child.StartIndex || s.EndIndex == 0));

                if (nextSpan == null)
                {
                    parent.Spans.Add(child);
                    break;
                }

                parent = nextSpan;
            }
        }

        private Span FindSpanStart(string markdownString)
        {
            var tag = MatchTag(markdownString, t => t.MarkdownStart);
            if (tag == null)
                return null;

            var startIndex = index;
            index = index + tag.MarkdownStart.Length - 1;
            return new Span(tag, startIndex);
            
        }
        private Span FindSpanEnd(string markdownString, List<Span> openedSpans)
        {
            var tag = MatchTag(markdownString, t => t.MarkdownEnd);
            if (tag == null)
                return null;

            foreach (var openedSpan in openedSpans)
            {
                if (Equals(openedSpan.Tag, tag))
                {
                    openedSpan.EndIndex = index;
                    index = index + openedSpan.Tag.MarkdownEnd.Length - 1;
                    return openedSpan;
                }

            }

            return null;
        }

        private Tag MatchTag(string markdownString, Func<Tag, string> param)
        {
            var possibleTags = new List<Tag>();
            foreach (var tag in tags)
            {
                var str = param(tag);
                var length = str.Length;
                if (markdownString.Length - index < length)
                    length = markdownString.Length - index;

                if (str == markdownString.Substring(index, length))
                    possibleTags.Add(tag);
            }

            return possibleTags.OrderByDescending(t => param(t).Length).FirstOrDefault();
        }
    }
}
