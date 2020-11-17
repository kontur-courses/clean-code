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
            markdownHeader = new Tag("# ", "\r\n");
            markdownToHtmlDictionary = new Dictionary<Tag, Tag>
            {
                {markdownBold, new Tag("<strong>", "</strong>")},
                {markdownItalic, new Tag("<em>", "</em>")},
                {markdownHeader, new Tag("<h1>", "</h1>\r\n")}
            };
        }

        public string ConvertToHtml(string markdown)
        {
            var replacements = FindReplacements(markdown);
            return ReplaceTags(markdown, replacements);
        }

        private List<Replacement> FindReplacements(string markdown)
        {
            var activeTags = new Dictionary<Tag, TagSubstring>();
            var replacements = new List<Replacement>();
            var index = 0;
            var hasSpace = false;
            var hasDigit = false;
            while (index < markdown.Length)
            {
                if (TryGetTag(markdown, index, activeTags, out var tagSubstring))
                {
                    HandleNewTag(markdown, tagSubstring, activeTags, hasDigit, hasSpace, replacements);
                    index += tagSubstring.Length;
                    hasDigit = false;
                    hasSpace = false;
                }
                else if (markdown[index] == '\\'
                         && (TryGetTag(markdown, index + 1, activeTags, out tagSubstring) ||
                             index + 1 < markdown.Length && markdown[index + 1] == '\\'))
                {
                    var replacement = markdown[index + 1] == '\\' ? "\\" : tagSubstring.Value;
                    replacements.Add(
                        new Replacement(
                            replacement,
                            new Substring(index, '\\' + replacement)));
                    index += replacement.Length + 1;
                }
                else if (markdown[index] == '\n' && activeTags.ContainsKey(markdownItalic) &&
                         activeTags.ContainsKey(markdownBold))
                {
                    activeTags.Remove(markdownItalic);
                    activeTags.Remove(markdownBold);
                }
                else
                {
                    if (markdown[index] == ' ')
                        hasSpace = true;
                    else if (char.IsDigit(markdown[index]))
                        hasDigit = true;
                    index++;
                }
            }
            return replacements;
        }

        private void HandleNewTag(string markdown, TagSubstring tagSubstring, Dictionary<Tag, TagSubstring> activeTags, bool hasDigit,
            bool hasSpace, List<Replacement> replacements)
        {
            if (tagSubstring.Role == TagRole.Opening)
            {
                if (tagSubstring.Tag == markdownHeader && (tagSubstring.Index == 0 || markdown[tagSubstring.Index - 1] == '\n')
                    || tagSubstring.EndIndex + 1 == markdown.Length
                    || markdown[tagSubstring.EndIndex + 1] != ' ')
                    activeTags.Add(tagSubstring.Tag, tagSubstring);
            }
            else if (tagSubstring.Tag == markdownHeader
                     || ValidateDefaultTag(markdown, activeTags[tagSubstring.Tag], tagSubstring, hasDigit, hasSpace))
            {
                if (CheckIfBoldTagInsideItalic(tagSubstring, activeTags))
                {
                    AddReplacement(replacements, activeTags[tagSubstring.Tag]);
                    AddReplacement(replacements, tagSubstring);
                }

                activeTags.Remove(tagSubstring.Tag);
            }
        }

        private bool CheckIfBoldTagInsideItalic(TagSubstring tagSubstring, Dictionary<Tag, TagSubstring> activeTags)
        {
            return tagSubstring.Tag != markdownBold || !activeTags.ContainsKey(markdownItalic);
        }

        private string ReplaceTags(string markdown, List<Replacement> replacements)
        {
            if (replacements.Count == 0)
                return markdown;
            replacements = replacements
                .OrderBy(replacement => replacement.OldValueSubstring.Index)
                .ToList();

            var resultBuilder = new StringBuilder();
            var firstTagIndex = replacements[0].OldValueSubstring.Index;
            if (firstTagIndex > 0)
                resultBuilder.Append(markdown.Substring(0, replacements[0].OldValueSubstring.Index));
            resultBuilder.Append(replacements[0].NewValue);

            for (var i = 1; i < replacements.Count; i++)
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

        private void AddReplacement(List<Replacement> replacements, TagSubstring substring)
        {
            replacements.Add(new Replacement(
                markdownToHtmlDictionary[substring.Tag].GetTagValue(substring.Role),
                substring));
        }

        private static bool ValidateDefaultTag(string markdown, TagSubstring firstTag, TagSubstring lastTag, bool hasDigit, bool hasSpace)
        {
            return markdown[lastTag.Index - 1] != ' '
                   && markdown[firstTag.EndIndex + 1] != ' '
                   && ((lastTag.EndIndex + 1 == markdown.Length || markdown[lastTag.EndIndex + 1] == ' ')
                       && (firstTag.Index - 1 < 0 || markdown[firstTag.Index - 1] == ' ')
                       || !hasDigit && !hasSpace);
        }

        private bool TryGetTag(string markdown, int index, Dictionary<Tag, TagSubstring> activeTags, out TagSubstring tagSubstring)
        {
            tagSubstring = markdownToHtmlDictionary
                .Select(tag => tag.Key)
                .Where(tag => TextContainsSubstring(markdown, index, tag.Opening)
                              || TextContainsSubstring(markdown, index, tag.Ending)
                                && activeTags.ContainsKey(tag))
                .Select(tag => TextContainsSubstring(markdown, index, tag.Opening) && !activeTags.ContainsKey(tag) ?
                    new TagSubstring(index, tag.Opening, tag, TagRole.Opening) :
                    new TagSubstring(index, tag.Ending, tag, TagRole.Ending))
                .FirstOrDefault();
            return tagSubstring != null;
        }

        private bool TextContainsSubstring(string text, int index, string substring)
        {
            if (text.Length < index + substring.Length)
                return false;
            for (var i = 0; i < substring.Length; i++)
            {
                if (text[index + i] != substring[i])
                    return false;
            }
            return true;
        }
    }
}