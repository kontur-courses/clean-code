using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MdConverter
    {
        private readonly IEnumerable<Token> tokens;
        private readonly string text;

        public MdConverter(IEnumerable<Token> tokens, string text)
        {
            this.tokens = tokens;
            this.text = text;
        }

        public IEnumerable<string> GetHtmlTokens()
        {
            return tokens.Select(token => ConvertToHtml(token)).ToList();
        }

        public string ConvertToHtml(Token token)
        {
            if (token.TagType is DefaultTagType || !HasPairTags(token))
                return text.Substring(token.Position, token.Length);
            return GetHtmlEquivalent(token);
        }

        public bool HasPairTags(Token token)
        {
            var type = token.TagType;
            var openingSymbol = type.MdOpeningTag;
            var closingSymbol = type.MdClosingTag;
            var tokenContent = text.Substring(token.Position, token.Length);
            return tokenContent.StartsWith(openingSymbol) && tokenContent.EndsWith(closingSymbol);
        }

        private string GetHtmlEquivalent(Token token)
        {
            var type = token.TagType;
            var openingSymbolLength = type.MdOpeningTag.Length;
            var closingSymbolLength = type.MdClosingTag.Length;
            var content = text.Substring(token.Position + openingSymbolLength,
                token.Length - closingSymbolLength - openingSymbolLength);
            return new StringBuilder(type.HtmlOpeningTag).Append(content).Append(type.HtmlClosingTag).ToString();
        }
    }
}