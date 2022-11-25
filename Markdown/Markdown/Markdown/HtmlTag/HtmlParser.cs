using Markdown.Markdown;
using Markdown.Tokens;
using System.Text;
using FluentAssertions;

namespace Markdown.HtmlTag
{
    public static class HtmlParser
    {
        
        public static string Parse(IList<Token> tokens, string md)
        {
            if (tokens.LastOrDefault().End + 1 != md.Length)
                throw new ArgumentOutOfRangeException("String for HTML parse have not all tokens");
            var htmlString = new StringBuilder();


            for (var index = 0; index < tokens.Count; index++)
            {
                var token = tokens[index];
                switch (token.Type)
                {
                    case TokenType.Image:
                        htmlString.Append($"<img src=\"{token.PathForImage}\" alt=\"{token.DescriptionForImage}\">");
                        break;
                    case TokenType.Default:
                        htmlString.Append(token.CreateString(md));
                        break;
                    case TokenType.Unseen:
                        break;
                    default:
                    {
                        var tag = GetTagFromToken(token);
                        htmlString.Append(token.Element == TokenElement.Open ? tag.StartTag : tag.EndTag);
                        break;
                    }
                }
            }

            return htmlString.ToString();
        }

        private static void AppendToken(string md, Token token, StringBuilder htmlString)
        {
             
           
            
        }

        public static HtmlTag GetTagFromToken(Token token)
        {
            return token.Type switch
            {
                TokenType.Strong => new HtmlTag("strong"),
                TokenType.Header => new HtmlTag("h1"),
                TokenType.Italic => new HtmlTag("em"),
                _ => throw new ArgumentException("Have not this tag")
            };
        }
    }
}
