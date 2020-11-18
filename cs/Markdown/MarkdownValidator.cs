using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class MarkdownValidator : ITagValidator
    {
        private readonly Tag markdownHeader, markdownBold, markdownItalic;

        public MarkdownValidator(Tag markdownHeader, Tag markdownBold, Tag markdownItalic)
        {
            this.markdownItalic = markdownItalic;
            this.markdownBold = markdownBold;
            this.markdownHeader = markdownHeader;
        }

        public bool ValidateOpeningTag(string markdown, TagSubstring tagSubstring)
        {
            return tagSubstring.Tag == markdownHeader && (tagSubstring.Index == 0 || markdown[tagSubstring.Index - 1] == '\n')
                   || tagSubstring.EndIndex + 1 == markdown.Length
                   || markdown[tagSubstring.EndIndex + 1] != ' ';
        }

        public bool ValidateEndingTag(string markdown, TagSubstring firstTag, TagSubstring lastTag, bool hasDigit, bool hasSpace)
        {
            return lastTag.Tag == markdownHeader
                   || markdown[lastTag.Index - 1] != ' '
                   && markdown[firstTag.EndIndex + 1] != ' '
                   && ((lastTag.EndIndex + 1 == markdown.Length || markdown[lastTag.EndIndex + 1] == ' ')
                       && (firstTag.Index - 1 < 0 || markdown[firstTag.Index - 1] == ' ')
                       || !hasDigit && !hasSpace);
        }

        public void HandleNewLine(Dictionary<Tag, TagSubstring> activeTags)
        {
            if (activeTags.ContainsKey(markdownItalic) &&
                activeTags.ContainsKey(markdownBold))
            {
                activeTags.Remove(markdownItalic);
                activeTags.Remove(markdownBold);
            }
        }
    }
}
