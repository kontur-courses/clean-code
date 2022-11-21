using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Enums;
using Markdown.Tokens;

namespace Markdown
{
    public static class HtmlConverter
    {
        public static Dictionary<TokenType, string> TagHtmlDictionary = new Dictionary<TokenType, string>
        {
            [TokenType.Italic] = "em",
            [TokenType.Header] = "h1",
            [TokenType.Strong] = "strong"
        };
        public static string ConvertTokensToHtml(List<Token> tokens)
        {
            throw new ArgumentException();
        }
    }
}
