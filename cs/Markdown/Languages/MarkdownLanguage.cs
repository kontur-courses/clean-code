using System.Collections.Generic;
using Markdown.Tokenizing;

namespace Markdown.Languages
{
    public class MarkdownLanguage : Language
    {
        protected override Dictionary<Tag, string> OpeningTags =>
            new Dictionary<Tag, string> {{Tag.Emphasize, "_"}, {Tag.Strong, "__"}};

        protected override Dictionary<Tag, string> ClosingTags => OpeningTags;
    }
}
