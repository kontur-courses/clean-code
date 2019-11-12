using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private Dictionary<Token, Token> TagsCollection { get; set; }

        public Md()
        {
            TagsCollection = new Dictionary<Token, Token>();
        }
        public string Render(string paragraph)
        {
            var markdownTokens = TextToTokensParser.ParseTextToTokens(paragraph);
            var htmlTokens = MarkdownToHtmlParser.ParseMarkdownToHtmlTokens(markdownTokens, TagsCollection);
            var stringBuilder = new StringBuilder(paragraph);
            foreach (var markdownToken in markdownTokens)
                stringBuilder.Replace(markdownToken.Line, TagsCollection[markdownToken].Line, markdownToken
                    .Start, markdownToken.Length);
            return stringBuilder.ToString();
        }
    }
}