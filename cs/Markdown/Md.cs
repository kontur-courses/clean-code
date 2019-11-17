using System.Collections.Generic;
using System.Linq;
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
            var delta = 0;
            foreach (var markdownToken in markdownTokens.OrderBy(t=>t.Start))
            {
                stringBuilder.Replace(markdownToken.Line, TagsCollection[markdownToken].Line, markdownToken
                    .Start+delta, markdownToken.Length);
                delta += TagsCollection[markdownToken].Length - markdownToken.Length;
            }

            return stringBuilder.ToString();
        }
    }
}