using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class MdConvert
    {
        private readonly static Dictionary<TokenType, string> tokenPrefix;
        private readonly static Dictionary<TokenType, string> tokenSuffix;

        static MdConvert()
        {
            tokenPrefix = new Dictionary<TokenType, string> { { TokenType.Bold,  "<strong>" },
                                                                { TokenType.Italic, "<em>"},
                                                                { TokenType.Header, "<h1>"} ,
                                                                { TokenType.Simple, ""} };
            tokenSuffix = new Dictionary<TokenType, string> { { TokenType.Bold, "</strong>" },
                                                                { TokenType.Italic, "</em>" },
                                                                { TokenType.Header, "</h1>"} ,
                                                                { TokenType.Simple, ""} };

        }

        public static string ToHtml(IEnumerable<Token> tokens)
        {
            var converted = new StringBuilder();
            foreach (var token in tokens)
                converted.Append(TokenToHtml(token));
            return converted.ToString();
        }

        public static string TokenToHtml(Token token)
        {
            var prefix = tokenPrefix[token.type];
            var offset = prefix.Length;
            var convertedToken = new StringBuilder(prefix);
            convertedToken.Append(token.value);
            foreach (var nestedToken in token.NestedTokens)
            {
                convertedToken.Insert(nestedToken.Position + offset, TokenToHtml(nestedToken));
                offset += GetTotalTokenLength(nestedToken);
            }
            var suffix = tokenSuffix[token.type];
            convertedToken.Append(suffix);
            return convertedToken.ToString();
        }

        private static int GetTotalTokenLength(Token token)
        {
            return token.value.Length + tokenPrefix[token.type].Length + tokenSuffix[token.type].Length;
        }
    }
}
