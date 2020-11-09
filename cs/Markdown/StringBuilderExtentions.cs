using System.Text;

namespace Markdown
{
    public static class StringBuilderExtentions
    {
        public static StringBuilder ReplaceMarkdownTagsOnHtmlTags(this StringBuilder result, MarkdownTag tag)
        {
            return result.Replace(tag.Value, MarkdownTag.MatchingMarkdownTagsToHtmlTags[tag.Value].Item1, tag.Index, 1)
                .Replace(tag.Value, MarkdownTag.MatchingMarkdownTagsToHtmlTags[tag.Value].Item2, result.Length - 1, 1);
        }
    }
}