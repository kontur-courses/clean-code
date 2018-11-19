using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkdownParser : IParser
    {
        private List<Tag> tags;
        private int index;
        private string markdownString;

        public Span Parse(string rawString)
        {
            markdownString = rawString;
            tags = Markups.Markdown.Tags;
            index = 0;

            var mainSpan = new Span(Tag.Empty, 0, markdownString.Length);
            var openedSpans = new List<Span>();

            for (; index < markdownString.Length; index++)
            {
                if (markdownString[index] == '\\')
                {
                    mainSpan.PutSpan(new Span(Tag.Empty, index, index + 1));
                    index += 1;
                    continue;
                }

                if (openedSpans.Count != 0)
                {
                    var span = GetSpanEnd(openedSpans);
                    if (span != null)
                    {
                        openedSpans = openedSpans.Where(t => !t.IsClosed).ToList();
                        continue;
                    }
                }

                var newSpan = GetSpanStart();
                if (newSpan != null)
                {
                    mainSpan.PutSpan(newSpan);
                    openedSpans.Add(newSpan);
                }
            }

            mainSpan.RemoveNotClosedSpans();
            mainSpan.Segment();
            return mainSpan;
        }

        private bool CanBeStartTag(Tag tag)
        {
            if (tag == null)
                return false;

            if (tag.Open.Length + index < markdownString.Length)
            {
                var nextChar = markdownString[tag.Open.Length + index];

                if (char.IsWhiteSpace(nextChar) || char.IsDigit(nextChar))
                    return false;
            }

            return true;
        }

        private Span GetSpanStart()
        {
            var tag = MatchTag(t => t.Open);
            if (!CanBeStartTag(tag))
                return null;
            
            var startIndex = index;
            index = index + tag.Open.Length - 1;
            return new Span(tag, startIndex);

        }

        private bool CanBeEndTag(Tag openedTag, Tag tag)
        {
            if (!Equals(openedTag, tag))
                return false;
            if (index != 0 && char.IsWhiteSpace(markdownString[index - 1]))
                return false;
            if (index + tag.Close.Length != markdownString.Length &&
                char.IsDigit(markdownString[index + tag.Close.Length]))
                return false;

            return true;
        }
        private Span GetSpanEnd(IEnumerable<Span> openedSpans)
        {
            var tag = MatchTag(t => t.Close);
            if (tag == null)
                return null;

            foreach (var openedSpan in openedSpans)
            {
                if (!CanBeEndTag(openedSpan.Tag, tag))
                    continue;

                openedSpan.Close(index);
                index = index + openedSpan.Tag.Close.Length - 1;
                return openedSpan;

            }

            return null;
        }

        private Tag MatchTag(Func<Tag, string> param)
        {
            var possibleTags = new List<Tag>();
            foreach (var tag in tags)
            {
                var str = param(tag);
                if (markdownString.ContainsFrom(str, index))
                    possibleTags.Add(tag);
            }

            return possibleTags.OrderByDescending(t => param(t).Length).FirstOrDefault();
        }
    }
}
