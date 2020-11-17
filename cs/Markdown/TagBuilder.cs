using System.Linq;
using System.Text;

namespace Markdown
{
    public static class TagBuilder
    {
        public static Tag BuildTag(string text, int startIndex)
        {
            var mark = MarkdownTags.GetMarkFromText(startIndex, text);
            if (!MarkdownTags.IsMark(mark))
                return Tag.EmptyOn(startIndex);

            var endPos = FindClosePosition(text, startIndex, mark);
            var tagName = MarkdownTags.GetHtmlTagByMark(mark);


            return IsTagCorrect(startIndex, endPos, text)
                ? Tag.Correct(tagName, startIndex, endPos)
                : Tag.Incorrect(tagName, startIndex, endPos);
        }

        private static string GetContent(int startIndex, int endIndex, string text)
        {
            var contentBuilder = new StringBuilder();
            var mark = MarkdownTags.GetMarkFromText(startIndex, text);
            for (var i = startIndex + mark.Length; i <= endIndex - mark.Length; i++)
                contentBuilder.Append(text[i]);

            return contentBuilder.ToString();
        }

        private static int FindClosePosition(string text, int startIndex, string openedMark)
        {
            var i = startIndex + 1;
            var pairMark = MarkdownTags.GetMarkPair(openedMark);
            var mark = MarkdownTags.GetMarkFromText(i, text);
            while (i < text.Length - openedMark.Length + 1)
            {
                mark = MarkdownTags.GetMarkFromText(i, text);
                if (mark == pairMark && !char.IsWhiteSpace(text[i - 1]))
                    break;

                i += mark.Length;
            }

            return i == text.Length ? i : i + mark.Length - 1;
        }

        private static bool IsTagCorrect(int startIndex, int endIndex, string text)
        {
            var mark = MarkdownTags.GetMarkFromText(startIndex, text);
            var tagContent = GetContent(startIndex, endIndex, text);

            return mark == "#"
                   || endIndex < text.Length
                   && AreMarksEqualed(startIndex, endIndex, text)
                   && !tagContent.Any(char.IsDigit)
                   && !string.IsNullOrEmpty(tagContent)
                   && !AreMarksInsideDifferentWords(startIndex, endIndex, text);
        }

        private static bool AreMarksEqualed(int openedMarkIndex, int closingMarkIndex, string text)
        {
            var mark = MarkdownTags.GetMarkFromText(openedMarkIndex, text);

            return text.Substring(openedMarkIndex, mark.Length) ==
                   text.Substring(closingMarkIndex - mark.Length + 1, mark.Length);
        }

        private static bool AreMarkInsideWord(int position, string text)
        {
            return position + 1 < text.Length && position - 1 >= 0 && !char.IsWhiteSpace(text[position - 1]) &&
                   !char.IsWhiteSpace(text[position + 1]);
        }

        private static bool AreMarksInsideDifferentWords(int openedMarkIndex, int closingMarkIndex, string text)
        {
            var spaces = new[] {' ', '\t', '\n'};
            var content = text.Substring(openedMarkIndex, closingMarkIndex - openedMarkIndex);
            return AreMarkInsideWord(openedMarkIndex, text) && AreMarkInsideWord(closingMarkIndex, text) &&
                   content.Intersect(spaces).Any();
        }
    }
}