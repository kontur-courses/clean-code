using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownConverter
    {
        private readonly Dictionary<Tag, Tag> markdownToHtmlDictionary;
        private readonly Tag markdownBold, markdownItalic, markdownHeader;
        public MarkdownConverter()
        {
            markdownBold = new Tag("__", "__");
            markdownItalic = new Tag("_", "_");
            markdownHeader = new Tag("# ", "\n");
            markdownToHtmlDictionary = new Dictionary<Tag, Tag>
            {
                {markdownBold, new Tag("<strong>", "</strong>")},
                {markdownItalic, new Tag("<em>", "</em>")},
                {markdownHeader, new Tag("<h1>", "</h1>\n")}
            };
        }

        public string ConvertToHtml(string markdown)
        {
            var replacements = FindReplacements(markdown);
            return ReplaceTags(markdown, replacements);
        }

        private Replacement[] FindReplacements(string markdown)
        {
            var activeTags = new Dictionary<Tag, TagSubstring>();
            var replacements = new List<Replacement>();
            var i = 0;
            var hasSpace = false;
            var hasDigit = false;
            while (i < markdown.Length)
            {
                if (TryGetTag(markdown, i, out var substring, activeTags.Keys.ToHashSet()))
                {
                    if (substring.Role == TagRole.Opening)
                    {
                        if (substring.Tag == markdownHeader
                            || substring.EndIndex == markdown.Length - 1
                            || markdown[substring.EndIndex + 1] != ' ')
                            activeTags.Add(substring.Tag, substring);
                    }
                    else if (substring.Tag == markdownHeader
                             || ValidateDefaultTag(markdown, activeTags[substring.Tag], substring, hasDigit, hasSpace))
                    {
                        if (substring.Tag != markdownBold || !activeTags.ContainsKey(markdownItalic))
                        {
                            AddReplacement(replacements, activeTags[substring.Tag]);
                            AddReplacement(replacements, substring);
                        }
                        activeTags.Remove(substring.Tag);
                    }
                    i += substring.Length;
                    hasDigit = false;
                    hasSpace = false;
                }
                else if (markdown[i] == '\\'
                         && (TryGetTag(markdown, i + 1, out substring, activeTags.Keys.ToHashSet()) ||
                             i + 1 < markdown.Length && markdown[i + 1] == '\\'))
                {
                    var replacement = markdown[i + 1] == '\\' ? "\\" : substring.Value;
                    replacements.Add(
                        new Replacement(
                            replacement,
                            new Substring(i, '\\' + replacement)));
                    i += replacement.Length + 1;
                }
                else
                {
                    if (markdown[i] == ' ')
                        hasSpace = true;
                    else if (char.IsDigit(markdown[i]))
                    hasDigit = true;
                    i++;
                }
            }
            return replacements.ToArray();
        }

        private static bool ValidateDefaultTag(string markdown, TagSubstring firstTag, TagSubstring lastTag, bool hasDigit, bool hasSpace)
        {
            return markdown[lastTag.Index - 1] != ' '
                   && markdown[firstTag.EndIndex + 1] != ' '
                   && (lastTag.EndIndex + 1 < markdown.Length 
                       && markdown[lastTag.EndIndex + 1] == ' '
                       && firstTag.Index - 1 < markdown.Length 
                       && markdown[firstTag.Index - 1] == ' '
                       || !hasDigit && !hasSpace);
        }

        private void AddReplacement(List<Replacement> replacements, TagSubstring substring)
        {
            replacements.Add(new Replacement(
                markdownToHtmlDictionary[substring.Tag].GetTagValue(substring.Role),
                substring));
        }

        private string ReplaceTags(string markdown, Replacement[] replacements)
        {
            if (replacements.Length == 0)
                return markdown;
            replacements = replacements
                .OrderBy(replacement => replacement.OldValueSubstring.Index)
                .ToArray();
            var resultBuilder = new StringBuilder();
            var firstTagIndex = replacements[0].OldValueSubstring.Index;
            if (firstTagIndex > 0)
                resultBuilder.Append(markdown.Substring(0, replacements[0].OldValueSubstring.Index));
            resultBuilder.Append(replacements[0].NewValue);
            for (var i = 1; i < replacements.Length; i++)
            {
                var deltaStartIndex = replacements[i - 1].OldValueSubstring.EndIndex + 1;
                var deltaLength = replacements[i].OldValueSubstring.Index - deltaStartIndex;
                resultBuilder.Append(markdown.Substring(deltaStartIndex, deltaLength));
                resultBuilder.Append(replacements[i].NewValue);
            }
            var lastTagEndIndex = replacements[^1].OldValueSubstring.EndIndex + 1;
            if (lastTagEndIndex < markdown.Length)
                resultBuilder.Append(markdown.Substring(lastTagEndIndex, markdown.Length - lastTagEndIndex));
            return resultBuilder.ToString();
        }

        private bool TryGetTag(string markdown, int index, out TagSubstring substring, HashSet<Tag> activeTags)
        {
            substring = markdownToHtmlDictionary
                .Select(tag => tag.Key)
                .Where(tag => ContainsSubstring(markdown, index, tag.Opening)
                              || (ContainsSubstring(markdown, index, tag.Ending)
                                && activeTags.Contains(tag)))
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