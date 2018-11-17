using System.Collections.Generic;
using Markdown.Tokenizing;

namespace Markdown.Languages
{
    public class HtmlLanguage : Language
    {
        protected override Dictionary<Tag, string> OpeningTags =>
            new Dictionary<Tag, string> {{Tag.Emphasize, "<em>"}, {Tag.Strong, "<strong>"}};

        protected override Dictionary<Tag, string> ClosingTags =>
            new Dictionary<Tag, string> {{Tag.Emphasize, "</em>"}, {Tag.Strong, "</strong>"}};
    }
}
