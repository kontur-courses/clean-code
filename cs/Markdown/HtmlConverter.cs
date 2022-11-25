using System.Text;
using Markdown.Enums;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown
{
    public class HtmlConverter:IConverter
    {
        private readonly Dictionary<TokenType, string> TagHtmlDictionary = new Dictionary<TokenType, string>
        {
            [TokenType.Italic] = "em",
            [TokenType.Header] = "h1",
            [TokenType.Strong] = "strong"
        };
        public string ConvertTokens(List<Token> tokens)
        {
            var sb = new StringBuilder();
            var flag = false;
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Header)
                {
                    flag = true;
                    continue;
                }

                sb.Append(GetValue(token));
            }

            if (flag)
            {
                sb.Insert(0, "<h1>");
                sb.Append("</h1>");
            }

            return sb.ToString();
        }

        private string GetTag(Tag tag)
        {
            return tag.Status == TagStatus.Open
                ? $"<{TagHtmlDictionary[tag.Type]}>"
                : $"</{TagHtmlDictionary[tag.Type]}>";
        }

        private string GetValue(Token token)
        {
            if(token is Text text)
                return text.Value;

            return GetTag(token as Tag);
        }
    }
}
