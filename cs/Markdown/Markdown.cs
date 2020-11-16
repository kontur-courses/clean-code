using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdown)
        {
            var tokens = new TextParser().GetTokens(markdown);
            var parsedTokens = new List<Token>();
            var htmlConverter = new HtmlConverter();

            foreach (var token in tokens)
            {
                var childs = token.GetChildTokens(token.GetValueWithoutTags()).ToList();
                var newValue = htmlConverter.ConvertTokens(childs);

                token.Value = GetValueInMarkdownTags(token.Type, newValue);
                parsedTokens.Add(token);
            }

            return htmlConverter.ConvertTokens(parsedTokens);;
        }

        private static string GetValueInMarkdownTags(TokenType type, string newValue)
        {
            return type switch
            {
                TokenType.Strong => $"__{newValue}__",
                TokenType.Emphasized => $"_{newValue}_",
                TokenType.Heading => $"#{newValue}",
                TokenType.PlainText => newValue,
                _ => newValue
            };
        }
    }
}