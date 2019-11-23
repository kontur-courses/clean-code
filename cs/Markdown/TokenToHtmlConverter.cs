using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TokenToHtmlConverter
    {
        private static readonly Dictionary<string, string> ControlSymbolHtmlTags = new Dictionary<string, string>()
        {
            {"_", "em"},
            {"__", "strong"},
            {"", ""}
        };

        public string ConvertToHtml(Token token)
        {
            var innerTokens = token.InnerTokens;
            var value = token.Value;
            string tag;
            if (ControlSymbolHtmlTags.ContainsKey(token.Prefix))
                tag = ControlSymbolHtmlTags[token.Prefix];
            else
                throw new ArgumentException($"{token.Prefix} no such key in html base");
            var condition = token.Condition;
            if (innerTokens.IsEmpty)
            {
                return string.IsNullOrWhiteSpace(tag)
                    ? value.ToString()
                    : $"<{tag}>{value}</{tag}>";
            }

            var htmlString = new StringBuilder();
            while (!innerTokens.IsEmpty)
            {
                htmlString.Append(ConvertToHtml(innerTokens.RemoveFirst()));
            }

            if (condition == TokenPrefixCondition.Hide)
                return htmlString.ToString();

            if (!token.IsClosed)
                return token.Prefix + htmlString;
            return condition == TokenPrefixCondition.Tag
                ? $"<{tag}>{htmlString}</{tag}>"
                : $"{token.Prefix}{htmlString}{token.Prefix}";
        }
    }
}