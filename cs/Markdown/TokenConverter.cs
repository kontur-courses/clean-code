using System;

namespace Markdown
{
    public class TokenConverter
    {
        public static string ConvertTokenToHtml(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Raw:
                    return token.Content;
                case TokenType.Italic:
                    return $"<em>{token.Content}</em>";
                case TokenType.Bold:
                    return $"<strong>{token.Content}</strong>";
                default:
                    throw new ArgumentException("Unknown TokenType");
            }
        }
    }
}
