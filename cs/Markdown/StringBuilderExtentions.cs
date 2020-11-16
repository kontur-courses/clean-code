using System.Text;

namespace Markdown
{
    public static class StringBuilderExtentions
    {
        public static StringBuilder ReplaceMarkdownTagsOnHtmlTags(this StringBuilder result, MarkdownTag closedTag,
            MarkdownTag openedTag, ref int index)
        {
            var oldLength = result.Length;
            result = result
                .Replace(closedTag.Value, MarkdownTag.MatchingMarkdownTagsToHtmlTags[closedTag.Value].Item2,
                    closedTag.StartPosition, closedTag.Length)
                .Replace(openedTag.Value, MarkdownTag.MatchingMarkdownTagsToHtmlTags[openedTag.Value].Item1,
                    openedTag.StartPosition, openedTag.Length);
            index += result.Length - oldLength;
            return result;
        }

        public static StringBuilder ReplaceMarkdownTagsOnHtmlTags(this StringBuilder result, MarkdownTag openedTag)
        {
            return result
                .Append(MarkdownTag.MatchingMarkdownTagsToHtmlTags[openedTag.Value].Item2)
                .Remove(openedTag.StartPosition + 1, 1)
                .Replace(openedTag.Value, MarkdownTag.MatchingMarkdownTagsToHtmlTags[openedTag.Value].Item1,
                    openedTag.StartPosition, openedTag.Length);
        }

        public static StringBuilder ReplaceHtmlTagsOnMarkdownTags(this StringBuilder result, MarkdownTag tag, int start,
            int end, ref int index)
        {
            var oldLength = result.Length;
            result = result
                .Replace(MarkdownTag.MatchingMarkdownTagsToHtmlTags[tag.Value].Item1, tag.Value, start, end - start + 1)
                .Replace(MarkdownTag.MatchingMarkdownTagsToHtmlTags[tag.Value].Item2, tag.Value, start, end - start + 1);
            index -= oldLength - result.Length;
            return result;
        }
    }
}