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
        private readonly Tag emptyTag = new Tag(TagValue.None, "", "");
        private string markdownString;

        public Span Parse(string rawString)
        {
            markdownString = rawString;
            tags = Markups.Markdown.Tags;
            index = 0;

            var mainSpan = new Span(emptyTag, 0, markdownString.Length);
            var openedSpans = new List<Span>();

            for (; index < markdownString.Length; index++)
            {
                if (markdownString[index] == '\\')
                {
                    mainSpan.PutSpan(new Span(emptyTag, index, index + 1));
                    index += 1;
                    continue;
                }

                if (openedSpans.Count != 0)
                {
                    var span = FindSpanEnd(openedSpans);
                    if (span != null)
                    {
                        openedSpans = openedSpans.Where(t => !t.IsClosed).ToList();
                        continue;
                    }
                }

                var newSpan = FindSpanStart();
                if (newSpan != null)
                {
                    mainSpan.PutSpan(newSpan);
                    openedSpans.Add(newSpan);
                }
            }

            mainSpan.RemoveNotClosedSpans();
            return mainSpan;
        }

        private Span FindSpanStart()
        {
            var tag = MatchTag(t => t.Open);
            if (tag == null)
                return null;

            if (tag.Open.Length + index < markdownString.Length &&
                (char.IsWhiteSpace(markdownString[tag.Open.Length + index]) ||
                char.IsDigit(markdownString[tag.Open.Length + index])))
                return null;

            if (index > 0 && char.IsDigit(markdownString[index - 1]))
                return null;
            
            var startIndex = index;
            index = index + tag.Open.Length - 1;
            return new Span(tag, startIndex);

        }
        private Span FindSpanEnd(List<Span> openedSpans)
        {
            var tag = MatchTag(t => t.Close);
            if (tag == null)
                return null;

            foreach (var openedSpan in openedSpans)
            {
                if (Equals(openedSpan.Tag, tag) &&
                    (index == 0 || (!char.IsWhiteSpace(markdownString[index - 1]))) &&
                    (index + tag.Close.Length == markdownString.Length || (!char.IsDigit(markdownString[index + tag.Close.Length]))))
                {
                    openedSpan.Close(index);
                    index = index + openedSpan.Tag.Close.Length - 1;
                    return openedSpan;
                }

            }

            return null;
        }

        private Tag MatchTag(Func<Tag, string> param)
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
