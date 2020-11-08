using System.Text;

namespace Markdown
{
    public static class StringBuilderExtentions
    {
        public static StringBuilder ReplaceSymbolsOnTags(this StringBuilder result, Tag tag)
        {
            return result.Replace(tag.Value, Tag.MatchingSymbolsToTags[tag.Value].Item1, tag.Index, 1)
                .Replace(tag.Value, Tag.MatchingSymbolsToTags[tag.Value].Item2, result.Length - 1, 1);
        }
    }
}