using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownConverter
    {
        private readonly Dictionary<Tag, Tag> markdownToHtmlDictionary;
        private readonly HashSet<string> markdownTags;
        public MarkdownConverter()
        {
            markdownToHtmlDictionary = new Dictionary<Tag, Tag>
            {
                {new Tag("__", "__"), new Tag("<strong>", "</strong>")},
                {new Tag("_", "_"), new Tag("<em>", "</em>")},
                {new Tag("#", "\n"), new Tag("<h1>", "</h1>\n")},
            };
            markdownTags = markdownToHtmlDictionary
                .SelectMany(tagsPair => new string[] {tagsPair.Key.Opening, tagsPair.Key.Ending})
                .ToHashSet();
        }

        public string ConvertToHtml(string markdown)
        {
            var replacements = FindReplacements(markdown);
            return ReplaceTags(markdown, replacements);
        }

        private TagReplacement[] FindReplacements(string markdown)
        {
            var activeTags = new HashSet<Tag>();
            var replacements = new List<TagReplacement>();
            var i = 0;
            while (i < markdown.Length)
            {
                if (TryGetTag(markdown, i, out var substring, activeTags))
                {
                    if (substring.Role == TagRole.Opening)
                        activeTags.Add(substring.Tag);
                    replacements.Add(new TagReplacement(
                        markdownToHtmlDictionary[substring.Tag].GetTagValue(substring.Role),
                        substring));
                    i += substring.Length;
                }
                else i++;
            }
            return replacements.ToArray();
        }

        private string ReplaceTags(string markdown, TagReplacement[] replacements)
        {
            if (replacements.Length == 0)
                return markdown;
            var resultBuilder = new StringBuilder();
            if (replacements[0].OldTagSubstring.Index > 0)
                resultBuilder.Append(markdown.Substring(0, replacements[0].OldTagSubstring.Index));
            resultBuilder.Append(replacements[0].NewTag);
            for (var i = 1; i < replacements.Length; i++)
            {
                var startIndex = replacements[i - 1].OldTagSubstring.Index + replacements[i - 1].OldTagSubstring.Length;
                var length = replacements[i].OldTagSubstring.Index - startIndex;
                resultBuilder.Append(markdown.Substring(startIndex, length));
                resultBuilder.Append(replacements[i].NewTag);
            }

            var lastTagEndIndex = replacements[^1].OldTagSubstring.Index + replacements[^1].OldTagSubstring.Length;
            if (lastTagEndIndex < markdown.Length)
                resultBuilder.Append(markdown.Substring(lastTagEndIndex, markdown.Length - lastTagEndIndex));
            return resultBuilder.ToString();
        }

        private bool TryGetTag(string markdown, int index, out TagSubstring substring, HashSet<Tag> activeTags)
        {
            substring = markdownToHtmlDictionary
                .Select(tag => tag.Key)
                .Where(tag => ContainsSubstring(markdown, index, tag.Opening)
                    || ContainsSubstring(markdown, index, tag.Ending))
                .Select(tag => ContainsSubstring(markdown, index, tag.Opening) && !activeTags.Contains(tag) ?
                    new TagSubstring(tag.Opening, index, tag.Opening.Length, tag, TagRole.Opening) :
                    new TagSubstring(tag.Ending, index, tag.Ending.Length, tag, TagRole.Ending))
                .FirstOrDefault();
            return substring != null;
        }

        private bool ContainsSubstring(string text, int index, string substring)
        {
            return text.Length >= index + substring.Length 
                   && text.Substring(index, substring.Length) == substring;
        }
    }
}