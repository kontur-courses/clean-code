using System.Collections.Generic;
using Markdown.Converter;
using Markdown.Markup;
using Markdown.Tags;

namespace Markdown
{
    public class Md
    {
        private static readonly Dictionary<string, Tag> SupportedTags =
            new Dictionary<string, Tag>
            {
                {"_", new Tag("_", "em", "_")},
                {"__", new Tag("__", "strong", "__")},
                {"#", new Tag("#", "h1", "\n")}
            };

        public static string Render(string markupText)
        {
            if (markupText.Length == 0)
                return markupText;
            
            var markupParser = new MarkupParser(SupportedTags);
            var markupParts = markupParser.ParseMarkup(markupText);

            return MarkupConverter.ConvertToHtmlString(markupParts);
        }
    }
}