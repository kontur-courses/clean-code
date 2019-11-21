using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TokenTypesTranslator
    {
        private static readonly Dictionary<string, Token> StringToMdToken = new Dictionary<string, Token>()
        {
            {"_", new PairToken(TokenType.Italic, "_")},
            {"__", new PairToken(TokenType.Bold, "__")},
            {"#", new HeaderToken(TokenType.H1, "#")},
            {"##", new HeaderToken(TokenType.H2, "##")},
            {"###", new HeaderToken(TokenType.H3, "###")},
            {"####", new HeaderToken(TokenType.H4, "####")},
            {"#####", new HeaderToken(TokenType.H5, "#####")},
            {"######", new HeaderToken(TokenType.H6, "######")},
        };

        private static readonly Dictionary<TokenType, string> TokenTypeToString =
            StringToMdToken.ToDictionary(element => element.Value.Type, element => element.Key);

        private static readonly Dictionary<TokenType, HTMLTag> MdTokenTypeToHtmlTag =
            new Dictionary<TokenType, HTMLTag>()
            {
                {TokenType.Bold, new HTMLTag("strong")},
                {TokenType.Italic, new HTMLTag("em")},
                {TokenType.H1, new HTMLTag("h1")},
                {TokenType.H2, new HTMLTag("h2")},
                {TokenType.H3, new HTMLTag("h3")},
                {TokenType.H4, new HTMLTag("h4")},
                {TokenType.H5, new HTMLTag("h5")},
                {TokenType.H6, new HTMLTag("h6")},
            };

        public static Token GetTokenFromString(string str)
        {
            return StringToMdToken[str];
        }

        public static string GetStringFromTokenType(TokenType tokenType)
        {
            return TokenTypeToString[tokenType];
        }

        public static HTMLTag GetHtmlTagFromTokenType(TokenType tokenType)
        {
            return MdTokenTypeToHtmlTag[tokenType];
        }

        public static List<string> GetSupportedTokens()
        {
            return StringToMdToken.Keys.ToList();
        }
    }
}