using Markdown.Tags;
using Markdown.Tokens;
using System.Text;

namespace Markdown.Generators
{
    public class HtmlGenerator : IMarkingGenerator
    {
        public string Generate(IEnumerable<IToken> tokens, string text)
        {
            var result = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Text)
                {
                    result.Append(token.Content);
                }
                else
                {
                    TagInfo.SupportedTags.TryGetValue(token.Content, out var tag);
                    result.Append(tag?.CreateHtmlTag(TagInfo.IsOpenTag(token, text)));
                }
            }

            return result.ToString();
        }
    }
}
