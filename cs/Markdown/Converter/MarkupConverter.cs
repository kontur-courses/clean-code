using System.Collections.Generic;
using System.Text;

namespace Markdown.Converter
{
    public static class MarkupConverter
    {
        public static string ConvertToHtmlString(IEnumerable<Markup.Markup> markupParts)
        {
            var stringBuilder = new StringBuilder();
            
            foreach (var markup in markupParts)
            {
                stringBuilder.Append(markup.Tag != null
                    ? $"<{markup.Tag.HtmlValue}>{markup.Value}</{markup.Tag.HtmlValue}>"
                    : markup.Value);
            }

            return stringBuilder.ToString();
        }
    }
}