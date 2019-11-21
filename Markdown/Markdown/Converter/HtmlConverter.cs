using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {

        private static readonly Dictionary<AttributeType, string> tagDictionary = new Dictionary<AttributeType, string>
        {
            {AttributeType.Emphasis, "em"},
            {AttributeType.Strong, "strong"},
            {AttributeType.Escape, ""},
            {AttributeType.None, ""},
            {AttributeType.Link, "a"},
        };


        public string ReplaceAttributesWithTags(TokenText tokenText)
        {
            var textPosition = 0;
            var sb = new StringBuilder();
            foreach (var token in tokenText.InTextTokens)
            {
                sb.Append(tokenText.Source.Substring(textPosition, token.Position - textPosition));
                sb.Append(GetTag(token));
                textPosition = token.Position + token.AttributeLength;
            }

            sb.Append(tokenText.Source.Substring(textPosition, tokenText.Source.Length - textPosition));

            return sb.ToString();
        }

        private string GetTag(IToken token)
        {
            if (token is LinkToken linkToken)
                return GenerateTagFromLinkToken(linkToken);

            var tagName = tagDictionary[token.Type];
            return token is PairToken pairToken ? $"<{(pairToken.IsClosing ? "/" : "")}{tagName}>" : tagName;
        }

        private string GenerateTagFromLinkToken(LinkToken linkToken)
        {
            if (linkToken.RawUrl == null)
                return $"</{tagDictionary[linkToken.Type]}>";

            var tokenizer = new Tokenizer(Syntax.InLinkSyntax);
            var url = ReplaceAttributesWithTags(tokenizer.ParseText(linkToken.RawUrl));

            return $"<{tagDictionary[linkToken.Type]} href=\"{url}\">";
        }
    }
}