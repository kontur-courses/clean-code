using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class HtmlCreator
    {
        public static readonly Dictionary<Styles, string> StylesWithHtmlTags = new Dictionary<Styles, string>
        {
            {Styles.Italic, "em"}, {Styles.Bold, "strong"}, {Styles.Title, "h1"}
        };

        public static string AddHtmlTagToText(string text, Token token)
        {
            var textStringBuilder = new StringBuilder(text);
            var tag = StylesWithHtmlTags[token.Style];
            if (token.EndPosition != null)
            {
                if (token.EndPosition < text.Length)
                    textStringBuilder.Replace($"{token.Separator}{token.Value}{token.Separator}",
                        $"<{tag}>{token.Value}</{tag}>");
                else
                    textStringBuilder.Replace($"{token.Separator}{token.Value}",
                        $"<{tag}>{token.Value}</{tag}>");
            }

            return textStringBuilder.ToString();
        }
    }
}