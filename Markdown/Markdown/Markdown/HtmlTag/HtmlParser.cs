using Markdown.Markdown;
using Markdown.Tokens;
using System.Text;

namespace Markdown.HtmlTag
{
    public static class HtmlParser
    {
        public static string Parse(IList<Token> tokens, string md)
        {
            if (tokens.LastOrDefault().End + 1 != md.Length)
                throw new ArgumentOutOfRangeException("String for HTML parse have not all tokens");
            var htmlString = new StringBuilder();
            foreach (var token in tokens)
                AppendToken(md, token, htmlString);
            return htmlString.ToString();
        }

        private static void AppendToken(string md, Token token, StringBuilder htmlString)
        {
            switch (token.Type)
            {
                case TokenType.Default:
                    htmlString.Append(token.CreateString(md));
                    return;
                case TokenType.Unseen:
                    return;
                default:
                    {
                        var tag = GetTagFromToken(token);
                        htmlString.Append(token.Element == TokenElement.Open ? tag.StartTag : tag.EndTag);
                        break;
                    }
            }
        }

        public static HtmlTag GetTagFromToken(Token token)
        {
            return token.Type switch
            {
                TokenType.Strong => new HtmlTag("strong"),
                TokenType.Header => new HtmlTag("h1"),
                TokenType.Italic => new HtmlTag("em"),
                TokenType.Image => new HtmlTag("<img src=\"", "\">"),
                _ => throw new ArgumentException("Have not this tag")
            };
        }
    }
}
