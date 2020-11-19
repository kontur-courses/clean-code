using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class MarkdownValidator : ITagValidator
    {
        private readonly HashSet<Tag> tagsUnderValidation,  tagsOutOfValidation;

        public MarkdownValidator(HashSet<Tag> markdownTags, HashSet<Tag> tagsOutOfValidation)
        {
            if (!tagsOutOfValidation.IsProperSubsetOf(markdownTags))
                throw new ArgumentException();
            this.tagsOutOfValidation = tagsOutOfValidation;
            tagsUnderValidation = markdownTags.Except(tagsOutOfValidation).ToHashSet();
        }

        public bool ValidateOpeningTag(string markdown, TagSubstring tagSubstring)
        {
            return tagsOutOfValidation.Contains(tagSubstring.Tag) && (tagSubstring.Index == 0 || markdown[tagSubstring.Index - 1] == '\n')
                   || tagSubstring.EndIndex + 1 == markdown.Length
                   || markdown[tagSubstring.EndIndex + 1] != ' ';
        }

        public bool ValidateEndingTag(string markdown, TagSubstring firstTag, TagSubstring lastTag, bool hasDigit, bool hasSpace)
        {
            return tagsOutOfValidation.Contains(lastTag.Tag)
                   || markdown[lastTag.Index - 1] != ' '
                   && markdown[firstTag.EndIndex + 1] != ' '
                   && ((lastTag.EndIndex + 1 == markdown.Length || markdown[lastTag.EndIndex + 1] == ' ')
                       && (firstTag.Index - 1 < 0 || markdown[firstTag.Index - 1] == ' ')
                       || !hasDigit && !hasSpace);
        }

        public void HandleNewLine(Dictionary<Tag, TagSubstring> activeTags)
        {
            if (tagsUnderValidation.All(activeTags.ContainsKey))
            {
                foreach (var tag in tagsUnderValidation)
                {
                    activeTags.Remove(tag);
                }
            }
        }
    }
}
