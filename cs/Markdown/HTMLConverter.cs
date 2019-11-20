using Markdown.Styles;
using Markdown.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal class HTMLConverter : ITextConverter
    {
        public string Convert(List<Token> tokens)
        {
            var sb = new StringBuilder();
            foreach (var token in tokens)
                sb.Append(token is StyleToken styleToken ? styleToken.ToHtml() : token.ToText());
            return sb.ToString();
        }

        public List<string> ConvertToHtmlPage(IEnumerable<string> lines)
        {
            var html = new List<string>();
            html.Add(@"<html><head><title>ConvertToHtmlPage</title></head><body>");
            html.AddRange(lines.Select(l => $"<p>{l}</p>"));
            html.Add(@"</body></html>");
            return html;
        }
    }

    internal static class StylesExtentions
    {
        public static string ToHtml(this StyleToken styleToken)
        {
            string tag = HtmlTag(styleToken.StyleType);
            if (styleToken is StyleEndToken) 
                return $"</{tag}>";
            else 
                return $"<{tag}>";
        }

        private static string HtmlTag(Type styleType)
        {
            if (styleType == typeof(Italic)) return "em";
            if (styleType == typeof(Bold)) return "strong";
            return nameof(styleType);
        }
    }
}
