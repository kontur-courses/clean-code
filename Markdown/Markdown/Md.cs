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
        private int i;
        private Tag emptyTag = new Tag("", "", "", "");

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
            i = 0;
            return ParseWithoutRegexp(markdownString);
        }

        private string ParseWithoutRegexp(string markdownString)
        {
            var mainSpan = new Span(emptyTag, 0, markdownString.Length - 1);
            var openedSpans = new List<Span>();

            for (; i < markdownString.Length; i++)
            {
                
                if (markdownString[i] == '\\')
                {
                    mainSpan.Spans.Add(new Span(emptyTag, i, i + 1));
                    i += 1;
                    continue;
                }

                if (openedSpans.Count != 0)
                {
                    var span = FindSpanEnd(markdownString, openedSpans);
                    if (span != null)
                    {
                        PutSpanInSpan(span, mainSpan);

                        openedSpans = openedSpans.Where(t => t.EndIndex == 0).ToList();
                        continue;
                    }
                }

                var newSpan = FindSpanStart(markdownString);
                if (newSpan != null)
                {
                    if (openedSpans.Count != 0)
                    {
                        openedSpans = openedSpans.OrderByDescending(s => s.StartIndex).ToList();
                        openedSpans[0].Spans.Add(newSpan);
                    }

                    openedSpans.Add(newSpan);
                }
            }
            
            return mainSpan.Assembly(markdownString);
        }

        private void PutSpanInSpan(Span child, Span parent)
        {

            var parentSpan = parent;
            while (true)
            {
                if (parentSpan.Spans.Count == 0)
                {
                    parentSpan.Spans.Add(child);
                    break;
                }

                var nextSpan = parentSpan.Spans.FirstOrDefault(
                    s => s.StartIndex < child.StartIndex && s.EndIndex > child.EndIndex);

                if (nextSpan == null)
                {
                    parentSpan.Spans.Add(child);
                    break;
                }

                parentSpan = nextSpan;
            }
        }

        private Span FindSpanStart(string markdownString)
        {
            var tag = MatchTag(markdownString, t => t.MarkdownStart);
            if (tag == null)
                return null;

            var startIndex = i;
            i = i + tag.MarkdownStart.Length - 1;
            return new Span(tag, startIndex);
            
        }
        private Span FindSpanEnd(string markdownString, List<Span> openedSpans)
        {
            var tag = MatchTag(markdownString, t => t.MarkdownEnd);
            if (tag == null)
                return null;

            foreach (var openedSpan in openedSpans)
            {
                //странно что tag.equals не работает
                if (openedSpan.Tag.MarkdownEnd == tag.MarkdownEnd)
                {
                    openedSpan.EndIndex = i;
                    i = i + openedSpan.Tag.MarkdownEnd.Length - 1;
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
                if (markdownString.Length - i < length)
                    length = markdownString.Length - i;

                if (str == markdownString.Substring(i, length))
                    possibleTags.Add(tag);
            }

            return possibleTags.Count == 0 ? null : possibleTags.OrderByDescending(t => param(t).Length).First();
        }
    }
}
