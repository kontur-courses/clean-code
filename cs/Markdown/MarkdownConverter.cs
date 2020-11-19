using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkdownConverter
    {
        private readonly Dictionary<Tag, Tag> markdownToHtmlDictionary;
        private readonly ITagValidator validator;

        public MarkdownConverter()
        {
            markdownToHtmlDictionary = new Dictionary<Tag, Tag>
            {
                {new Tag("__", "__", new HashSet<Tag> { new Tag("_", "_"), new Tag("![](", ")")}), 
                    new Tag("<strong>", "</strong>")},
                {new Tag("_", "_", new HashSet<Tag>{new Tag("![](", ")")}),
                    new Tag("<em>", "</em>")},
                { new Tag("# ", "\r\n", 
                    new HashSet<Tag>{new Tag("![](", ")")}), new Tag("<h1>", "</h1>\r\n")},
                { new Tag("![](", ")"), new Tag("<img src=\"", "\">")},
            };
            validator = new MarkdownValidator(
                markdownToHtmlDictionary.Keys.ToHashSet(), 
                new HashSet<Tag>{ new Tag("# ", "\r\n"), new Tag("![](", ")" )});
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
                else if (HasShieldingAtIndex(markdown, index, activeTags, ref tagSubstring))
                    AddShieldedReplacement(markdown, ref index, tagSubstring, replacements);
                else
                {
                    if (markdown[index] == '\n')
                        validator.HandleNewLine(activeTags);
                    if (markdown[index] == ' ')
                        hasSpace = true;
                    else if (char.IsDigit(markdown[index]))
                        hasDigit = true;
                    index++;
                }
            }
            return replacements;
        }

        private bool HasShieldingAtIndex(string markdown, int index, Dictionary<Tag, TagSubstring> activeTags, ref TagSubstring tagSubstring)
        {
            return markdown[index] == '\\'
                   && (TryGetTag(markdown, index + 1, activeTags, out tagSubstring) ||
                       index + 1 < markdown.Length && markdown[index + 1] == '\\');
        }

        private static void AddShieldedReplacement(string markdown, ref int index, TagSubstring tagSubstring, List<Replacement> replacements)
        {
            var replacement = markdown[index + 1] == '\\' ? "\\" : tagSubstring.Value;
            replacements.Add(
                new Replacement(
                    replacement,
                    new Substring(index, '\\' + replacement)));
            index += replacement.Length + 1;
        }

        private void HandleNewTag(string markdown, TagSubstring tagSubstring, Dictionary<Tag, TagSubstring> activeTags, bool hasDigit,
            bool hasSpace, List<Replacement> replacements)
        {
            if (tagSubstring.Role == TagRole.Opening)
            {
                if (validator.ValidateOpeningTag(markdown, tagSubstring))
                    activeTags.Add(tagSubstring.Tag, tagSubstring);
            }
            else if (validator
                         .ValidateEndingTag(markdown, activeTags[tagSubstring.Tag], tagSubstring, hasDigit, hasSpace))
            {
                if (tagSubstring.Tag.CheckForContext(activeTags))
                {
                    AddReplacement(replacements, activeTags[tagSubstring.Tag]);
                    AddReplacement(replacements, tagSubstring);
                }

                activeTags.Remove(tagSubstring.Tag);
            }
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

        private bool TryGetTag(string markdown, int index, Dictionary<Tag, TagSubstring> activeTags, out TagSubstring tagSubstring)
        {
            tagSubstring = null;
            bool MarkdownContainsSubstring(string substring) => TextContainsSubstring(markdown, index, substring);
            var newTag = markdownToHtmlDictionary.Keys
                .FirstOrDefault(tag => MarkdownContainsSubstring(tag.Opening)
                                       || MarkdownContainsSubstring(tag.Ending)
                                       && activeTags.ContainsKey(tag));
            if (newTag == null)
                return false;
            tagSubstring = MarkdownContainsSubstring(newTag.Opening) && !activeTags.ContainsKey(newTag)
                ? TagSubstring.FromOpening(index, newTag)
                : TagSubstring.FromEnding(index, newTag);
            return true;
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