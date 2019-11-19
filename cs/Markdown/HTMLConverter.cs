using Markdown.Styles;
using Markdown.Tokens;
using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class HTMLConverter : ITextConverter
    {
        public string Convert(List<Token> tokens)
        {
            string result = string.Empty;
            foreach (var token in tokens)
            {
                if (token is StyleToken styleToken)
                    result += styleToken.ToHtml();
                else
                    result += token.ToText();
            }
            return result;
        }
    }

    internal static class StylesExtentions
    {
        public static string ToHtml(this StyleToken styleToken)
        {
            string tag = HtmlTag(styleToken.StyleType);
            if (styleToken is StyleEndToken) 
                return $"<{tag}/>";
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
