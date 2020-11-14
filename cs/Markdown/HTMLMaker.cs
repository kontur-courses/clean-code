using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class HtmlMaker
    {
        private static readonly Dictionary<Tag, string> OpeningTag = new Dictionary<Tag, string>
        {
            {Tag.Bold, "<strong>"},
            {Tag.Italic, "<em>"},
            {Tag.Heading, "<h1>"},
            {Tag.NoFormatting, ""}
        };
        
        private static readonly Dictionary<Tag, string> ClosingTag = new Dictionary<Tag, string>
        {
            {Tag.Bold, "</strong>"},
            {Tag.Italic, "</em>"},
            {Tag.Heading, "</h1>"},
            {Tag.NoFormatting, ""}
        };
        
        public static string FromTextInfo(TextInfo textInfo)
        {
            var result = new StringBuilder();
            result.Append(OpeningTag[textInfo.Tag]);
            result.Append(textInfo.Text);
            foreach (var content in textInfo.Content)
                result.Append(FromTextInfo(content));
            result.Append(ClosingTag[textInfo.Tag]);

            return result.ToString();
        }
    }
}