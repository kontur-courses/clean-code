using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MdConverter : Converter
    {
        private string text;

        public override string GetHtml(List<Token> tokens, string text)
        {
            this.text = text;
            var htmlParts = tokens.Select(ConvertToHtml).ToList();
            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].NestedTokenCount > 0 && tokens[i].TagType is StrongTagType)
                {
                    for (var j = 0; j < tokens[i].NestedTokenCount; j++)
                    {
                        var content = text.Substring(tokens[j].Position, tokens[j].Length);
                        htmlParts[i] = htmlParts[i].Replace(content, htmlParts[i - tokens[i].NestedTokenCount + j]);
                        htmlParts[i - tokens[i].NestedTokenCount + j] = string.Empty;
                    }
                }
            }

            return string.Join("", htmlParts.Where(htmlPart => htmlPart != string.Empty));
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
            var openingSymbol = type.GetOpenedTag(Tag.Markup.Md);
            var closingSymbol = type.GetClosedTag(Tag.Markup.Md);
            var tokenContent = text.Substring(token.Position, token.Length);
            return tokenContent.StartsWith(openingSymbol) && tokenContent.EndsWith(closingSymbol);
        }

        private string GetHtmlEquivalent(Token token)
        {
            var type = token.TagType;
            var content = GetTokenContent(token);
            return new StringBuilder(type.GetOpenedTag(Tag.Markup.Html)).Append(content)
                .Append(type.GetClosedTag(Tag.Markup.Html)).ToString();
        }

        private string GetTokenContent(Token token)
        {
            var type = token.TagType;
            var openingSymbolLength = type.GetOpenedTag(Tag.Markup.Md).Length;
            var closingSymbolLength = type.GetClosedTag(Tag.Markup.Md).Length;
            return text.Substring(token.Position + openingSymbolLength,
                token.Length - closingSymbolLength - openingSymbolLength);
        }
    }
}