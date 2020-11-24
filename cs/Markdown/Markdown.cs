using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown
{
    public class Markdown
    {
        private static readonly Dictionary<string, Tag> SupportedTags =
            new Dictionary<string, Tag>
            {
                {"_", new Tag("_", "em", "_")},
                {"__", new Tag("__", "strong", "__")},
                {"# ", new Tag("# ", "h1", "\n")}
            };

        private static readonly HashSet<char> ShieldedSigns = new HashSet<char>
        {
            '_', '#', '\\'
        };

        public static string Render(string markupText)
        {
            if (string.IsNullOrEmpty(markupText) || markupText.Length < 2)
                return markupText;

            var mp = new MarkupParser.MarkupParser(SupportedTags, ShieldedSigns);
            return mp.ParseMarkup(markupText);
        }
    }
}