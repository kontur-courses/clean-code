using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class MarkdownParser : IParser
    {
        private IReadOnlyList<Tag> tags =  Markups.Markdown.Tags;
        private int index;
        private string markdownString;
        private Span mainSpan;
        private List<Span> openedSpans;

        private void SetUp(string rawString)
        {
            markdownString = rawString;
            index = 0;
            mainSpan = new Span(Tag.Empty, 0, markdownString.Length);
            openedSpans = new List<Span>();
        }
        public Span Parse(string rawString)
        {
            SetUp(rawString);

            for (; index < markdownString.Length; index++)
            {
                if (TryFindBackslash())
                    continue;
                if (TryCloseSpan())
                    continue;

                TryBeginSpan();
            }

            mainSpan.RemoveNotClosedSpans();
            mainSpan.Segment();

            return mainSpan;
        }

        private bool TryFindBackslash()
        {
            if (markdownString[index] != '\\')
                return false;

            var ignoredSpan = new Span(Tag.Empty, index, index + 1) {IsIgnored = true};
            mainSpan.PutSpan(ignoredSpan);
            index += 1;

            return true;
        }

        private bool CanBeStartTag(Tag tag)
        {
            if (tag == null)
                return false;

            if (tag.Open.Length + index >= markdownString.Length)
                return true;

            var nextChar = markdownString[tag.Open.Length + index];
            return !char.IsWhiteSpace(nextChar) && !char.IsDigit(nextChar);
        }

        private bool CanBeEndTag(Tag openedTag, Tag tag)
        {
            if (!Equals(openedTag, tag))
                return false;

            if (index != 0 && char.IsWhiteSpace(markdownString[index - 1]))
                return false;

            return index + tag.Close.Length == markdownString.Length || !char.IsDigit(markdownString[index + tag.Close.Length]);
        }

        private void TryBeginSpan()
        {
            var tag = MatchTag(t => t.Open).OrderByDescending(t => t.Open.Length).FirstOrDefault();
            if (!CanBeStartTag(tag))
                return;
            
            var startIndex = index;
            index = index + tag.Open.Length - 1;
            var newSpan =  new Span(tag, startIndex);
            mainSpan.PutSpan(newSpan);
            openedSpans.Add(newSpan);
        }

        private bool TryCloseSpan()
        {
            if (openedSpans.Count == 0)
                return false;
            
            var possibleTags = MatchTag(t => t.Close);
            if (possibleTags == null)
                return false;

            foreach (var openedSpan in openedSpans.OrderByDescending(s => s.Tag.Close.Length))
            {
                foreach (var possibleTag in possibleTags)
                {
                    if (!CanBeEndTag(openedSpan.Tag, possibleTag))
                        continue;

                    openedSpan.Close(index);
                    index = index + openedSpan.Tag.Close.Length - 1;
                    openedSpans = openedSpans.Where(t => !t.IsClosed && !openedSpan.Children.Contains(t)).ToList();
                    openedSpan.RemoveNotClosedSpans();
                    return true;
                }
            }

            return false;
        }

        private List<Tag> MatchTag(Func<Tag, string> param)
        {
            return tags.Where(tag => markdownString.ContainsFrom(param(tag), index)).ToList();
        }
    }
}
