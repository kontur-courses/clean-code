using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class HtmlParser
    {
        public static string Parse(IList<Token> tokens,string md)
        {
            var htmlString= new StringBuilder();
            
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Default)
                {
                    htmlString.Append(token.CreateString(md));
                    continue;
                }
                var tag = GetTagFromToken(token);
                if (token.Element == TokenElement.Open)
                {
                    htmlString.Append(tag.StartTag);
                }
                else
                {
                    htmlString.Append(tag.EndTag);
                }
            }

            return htmlString.ToString();
        }

        public static HtmlTag GetTagFromToken(Token token)
        {
            return token.Type switch
            {
                TokenType.Strong => new HtmlTag("strong"),
                TokenType.Header => new HtmlTag("header"),
                TokenType.Italic => new HtmlTag("em"),
                _ => throw new ArgumentException("Have not this tag")
            };
        }
    }
}
