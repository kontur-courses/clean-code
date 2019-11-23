using System.Collections.Generic;
using System.Linq;
using MarkDown.TagParsers;

namespace MarkDown
{
    public class LineConverter
    {
        private readonly List<TagParser> htmlParsers;
        private readonly ParserGetter parsers;

        public LineConverter(ParserGetter parserGetter)
        {
            parsers = parserGetter;
            htmlParsers = parsers.GetOrderedTagParsers();
        }

        public string GetParsedLineFrom(string line)
        {
            var orderedEscapeIndexes = new Queue<int>();
            var orderedTags = new Queue<Tag>();

            for (var i = 0; i < line.Length; i++)
                if (parsers.EscapeSymbols.Contains(line[i]))
                {
                    orderedEscapeIndexes.Enqueue(i);
                    i++;
                }
                else if (parsers.FirstTagChars.Contains(line[i]))
                {
                    if (TryToGetTag(line, i, out var tag))
                    {
                        i = tag.IndexNextToTag;
                        orderedTags.Enqueue(tag);
                    }
                }

            var orderedTagSubsequence = new Queue<Tag>(FindRightTagSubsequence(orderedTags));
            var ignoredTags = new Queue<Tag>(FindIgnoredTags(orderedTagSubsequence).ToList());

            var parser = new MdToHtmlParser(parsers);
            var html = parser.GetHtml(line, orderedTagSubsequence, ignoredTags, orderedEscapeIndexes);
            return html.ToString();
        }

        private bool TryToGetTag(string line, int startIndex, out Tag tag)
        {
            tag = null;
            var opening = htmlParsers.FirstOrDefault(p => p.TagIsOpening(line, startIndex));
            if (opening != null)
            {
                tag = new Tag(startIndex, opening.OpeningTags.md.Length, opening.Type);
                return true;
            }

            var closing = htmlParsers.FirstOrDefault(p => p.TagIsClosing(line, startIndex));
            if (closing == null)
                return false;
            tag = new Tag(startIndex, closing.ClosingTags.md.Length, closing.Type, false);
            return true;
        }

        private static IEnumerable<Tag> FindRightTagSubsequence(Queue<Tag> orderedTags)
        {
            var tagSubsequence = new List<Tag>();
            var tagStack = new Stack<Tag>();
            foreach (var tag in orderedTags)
            {
                if (tag.IsOpening)
                    tagStack.Push(tag);
                if (tag.IsOpening || tagStack.Count == 0) continue;
                var upperTag = tagStack.Peek();
                if (upperTag.Type != tag.Type) continue;
                tagSubsequence.Add(tagStack.Pop());
                tagSubsequence.Add(tag);
            }

            return tagSubsequence.OrderBy(tag => tag.StartIndex).ToList();
        }

        private IEnumerable<Tag> FindIgnoredTags(Queue<Tag> orderedTags)
        {
            var tagStack = new Stack<Tag>();
            foreach (var tag in orderedTags)
            {
                if (parsers.GetParserFromType(tag.Type).TagIgnorable(tagStack.Select(t => t.Type)))
                    yield return tag;

                if (tag.IsOpening)
                    tagStack.Push(tag);
                if (tag.IsOpening || tagStack.Count == 0) continue;
                var upperTag = tagStack.Peek();
                if (upperTag.Type == tag.Type) tagStack.Pop();
            }
        }
    }
}