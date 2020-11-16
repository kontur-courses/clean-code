using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownConverter
    {
        private readonly Dictionary<Tag, Tag> markdownToHtmlDictionary;

        public MarkdownConverter()
        {
            markdownToHtmlDictionary = new Dictionary<Tag, Tag>
            {
                {new Tag("__", "__"), new Tag("<strong>", "</strong>")},
                {new Tag("_", "_"), new Tag("<em>", "</em>")},
                {new Tag("#", "\n"), new Tag("<h1>", "</h1>\n")}
            };
        }

        public string ConvertToHtml(string markdown)
        {
            var replacements = FindReplacements(markdown);
            return ReplaceTags(markdown, replacements);
        }

        private Replacement[] FindReplacements(string markdown)
        {
            var activeTags = new Dictionary<Tag, Substring>();
            var replacements = new List<Replacement>();
            var i = 0;
            while (i < markdown.Length)
            {
                if (TryGetTag(markdown, i, out var substring, activeTags.Keys.ToHashSet()))
                {
                    if (substring.Role == TagRole.Opening)
                        activeTags.Add(substring.Tag, substring);
                    replacements.Add(new Replacement(
                        markdownToHtmlDictionary[substring.Tag].GetTagValue(substring.Role),
                        substring));
                    i += substring.Length;
                }
                else if (markdown[i] == '\\'
                         && (TryGetTag(markdown, i + 1, out var s, activeTags.Keys.ToHashSet()) ||
                             (i + 1 < markdown.Length && markdown[i + 1] == '\\')))
                {
                    var replacement = markdown[i + 1] == '\\' ? "\\" : s.Value;
                    replacements.Add(
                        new Replacement(
                            replacement,
                            new Substring(i, '\\' + replacement)));
                    i += replacement.Length + 1;
                }
                else i++;
            }
            return replacements.ToArray();
        }

        private string ReplaceTags(string markdown, Replacement[] replacements)
        {
            if (replacements.Length == 0)
                return markdown;
            var resultBuilder = new StringBuilder();
            var firstTagIndex = replacements[0].OldValueSubstring.Index;
            if (firstTagIndex > 0)
                resultBuilder.Append(markdown.Substring(0, replacements[0].OldValueSubstring.Index));
            resultBuilder.Append(replacements[0].NewValue);
            for (var i = 1; i < replacements.Length; i++)
            {
                var deltaStartIndex = replacements[i - 1].OldValueSubstring.EndIndex;
                var deltaLength = replacements[i].OldValueSubstring.Index - deltaStartIndex;
                resultBuilder.Append(markdown.Substring(deltaStartIndex, deltaLength));
                resultBuilder.Append(replacements[i].NewValue);
            }
            var lastTagEndIndex = replacements[^1].OldValueSubstring.EndIndex;
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
                .Select(tag => ContainsSubstring(markdown, index, tag.Opening) && !activeTags.Contains(tag)
                    ? new TagSubstring(index, tag.Opening, tag, TagRole.Opening)
                    : new TagSubstring(index, tag.Ending, tag, TagRole.Ending))
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