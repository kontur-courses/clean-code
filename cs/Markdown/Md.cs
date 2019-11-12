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
            var markdownTokens = TextToTokensParser.Parse(paragraph);
            var htmlTokens = MarkdownToHtmlParser.Parse(markdownTokens, TagsCollection);
            var stringBuilder = new StringBuilder(paragraph);
            //учесть изменение длины строки при замене md токена на html
            foreach (var markdownToken in markdownTokens)
                stringBuilder.Replace(markdownToken.Line, TagsCollection[markdownToken].Line, markdownToken
                    .Start, markdownToken.Length);
            return stringBuilder.ToString();
        }
    }
}