using System.Text;

namespace Markdown
{
    public static class StringBuilderExtentions
    {
        public static StringBuilder ReplaceMarkdownTagsOnHtmlTags(this StringBuilder result, MarkdownTag closedTag,
            MarkdownTag openedTag)
        {
            return result
                .Replace(closedTag.Value, MarkdownTag.MatchingMarkdownTagsToHtmlTags[closedTag.Value].Item2,
                    closedTag.StartPosition, closedTag.Length)
                .Replace(openedTag.Value, MarkdownTag.MatchingMarkdownTagsToHtmlTags[openedTag.Value].Item1,
                    openedTag.StartPosition, openedTag.Length);
        }

        public static StringBuilder ShieldSlash(this StringBuilder resultText, MarkdownTag closedSlash,
            MarkdownTag openedSlash)
        {
            return resultText.Replace(openedSlash.Value + closedSlash.Value, openedSlash.Value,
                openedSlash.StartPosition, openedSlash.Length + closedSlash.Length);
        }
    }
}