using System;
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

        private string ConvertToHtml(Token token)
        {
            if (token.TagType is DefaultTagType || !HasPairTags(token))
                return text.Substring(token.Position, token.Length);
            return GetHtmlEquivalent(token);
        }

        private bool HasPairTags(Token token)
        {
            var type = token.TagType;
            var openingSymbol = type.OpeningSymbol;
            var closingSymbol = type.ClosingSymbol;
            return text.Substring(token.Position, openingSymbol.Length) == openingSymbol &&
                   text.Substring(token.Position + token.Length - closingSymbol.Length + 1, closingSymbol.Length) ==
                   closingSymbol;
        }

        public string GetHtmlEquivalent(Token token)
        {
            var type = token.TagType;
            var openingSymbol = type.OpeningSymbol;
            var closingSymbol = type.ClosingSymbol;
            var content = text.Substring(token.Position + openingSymbol.Length, token.Length - closingSymbol.Length);
            return new StringBuilder(type.HtmlOpeningTag).Append(content).Append(type.HtmlClosingTag).ToString();
        }
    }
}