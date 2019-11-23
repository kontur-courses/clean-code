using System;
using System.Linq;

namespace Markdown
{
    public class HtmlTokenConverter : ITokenConverter
    {
        public string ConvertToken(FormattedToken formattedToken, string text)
        {
            if (formattedToken.Content == null)
                return ConvertTokenWithoutSubTokens(formattedToken, text);
            if (formattedToken.Type == FormattedTokenType.LinkText)
                return ConvertLinkToken(formattedToken, text);
            

            var contentSubStrings = formattedToken.Content
                .Select(t => ConvertToken(t, text));
            var content = string.Join("", contentSubStrings);
            switch (formattedToken.Type)
            {
                case FormattedTokenType.Raw:
                    return content;
                case FormattedTokenType.Italic:
                    return $"<em>{content}</em>";
                case FormattedTokenType.Bold:
                    return $"<strong>{content}</strong>";
                case FormattedTokenType.LinkUri:
                    return $"href=\"{content}\"";
                default:
                    throw new ArgumentException("Unknown TokenType");
            }
        }

        private string ConvertLinkToken(FormattedToken token, string text)
        {
            var uri = ConvertToken(token.Content
                .Single(t => t.Type == FormattedTokenType.LinkUri), text);
            var contentTokens = token.Content
                .Where(t => t.Type != FormattedTokenType.LinkUri).ToList();
            var content = ConvertToken(
                FormattedToken.GetTokenFromSubTokens(contentTokens, FormattedTokenType.Raw), 
                text);
            return $"<a {uri}>{content}</a>";
        }

        private static string ConvertTokenWithoutSubTokens(FormattedToken token, string text)
        {
            if (token.Content != null)
                throw new ArgumentException("Token shouldn't have sub tokens");
            return text.Substring(token.StartIndex, token.Length);
        }
    }
}
