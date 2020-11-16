using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdown)
        {
            var parser = new TextParser();
            var tokens = parser.GetTokens(markdown);
            var parsedTokens = new List<Token>();
            var htmlConverter = new HtmlConverter();
            var esc = parser.EscapingBackslashes;

            foreach (var token in tokens)
            {
                var childs = token.GetChildTokens(token.GetValueWithoutTags()).ToList();

                foreach (var child in childs)
                {
                    if (child.Type == TokenType.PlainText)
                    {
                        child.Value = RemoveEscapingBackslashes(child, esc);
                    }
                }

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

        public string RemoveEscapingBackslashes(Token token, List<int> esc)
        {
            var result = new StringBuilder();

            for (var i = 0; i < token.Value.Length; ++i)
            {
                if (!esc.Contains(token.Position + i))
                {
                    result.Append(token.Value[i]);
                }
            }

            return result.ToString();
        }
    }
}