using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        private static readonly Dictionary<AttributeType, string> tagDictionary = new Dictionary<AttributeType, string>
        {
            {AttributeType.Emphasis, "em"},
            {AttributeType.Strong, "strong"},
            {AttributeType.Escape, ""}
        };

        public string ReplaceAttributesWithTags(IEnumerable<Token> tokens, string source)
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

        private string GetTag(Token token)
        {
            var tagName = tagDictionary[token.Type];
            return token is PairToken pairToken ? $"<{(pairToken.IsClosing ? "/" : "")}{tagName}>" : tagName;
        }
    }
}