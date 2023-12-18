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
                    var tag = TagInfo.GetTagByMarkdownValue(token.Content);
                    result.Append(tag?.CreateHtmlTag(TagInfo.IsOpenTag(token, text)));
                }
            }

            return result.ToString();
        }
    }
}
