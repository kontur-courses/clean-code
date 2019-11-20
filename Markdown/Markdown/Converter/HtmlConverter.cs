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
            {AttributeType.LinkHeader, "head"},
            {AttributeType.LinkDescription, "desc"}
        };


        public string ReplaceAttributesWithTags(IEnumerable<IToken> tokens, string source)
        {
            var textPosition = 0;
            var sb = new StringBuilder();
            foreach (var token in tokens)
            {
                sb.Append(source.Substring(textPosition, token.Position - textPosition));
                sb.Append(GetTag(token));
                textPosition = token.Position + token.AttributeLength;
            }

            sb.Append(source.Substring(textPosition, source.Length - textPosition));

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
            var url = ReplaceAttributesWithTags(tokenizer.ParseText(linkToken.RawUrl), linkToken.RawUrl);

            return $"<{tagDictionary[linkToken.Type]} href=\"{url}\">";
        }
    }
}