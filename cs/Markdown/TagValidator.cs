using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    interface ITagValidator
    {
        public bool ValidateOpeningTag(string text, TagSubstring tag);
        public bool ValidateEndingTag(string text, TagSubstring firstTag, TagSubstring lastTag, bool hasDigit, bool hasSpace);
        void HandleNewLine(Dictionary<Tag, TagSubstring> activeTags);
    }
}
