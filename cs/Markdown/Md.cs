using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly Dictionary<Token, Token> tagsCollection;

        public Md()
        {
            tagsCollection = new Dictionary<Token, Token>();
        }
        public string Render(string paragraph)  
        {
            var markdownTokens = TextToTokensParser.Parse(paragraph);
            MarkdownToHtmlParser.Parse(markdownTokens, tagsCollection);
            var stringBuilder = new StringBuilder(paragraph);
            var delta = 0;
            foreach (var markdownToken in markdownTokens.OrderBy(t => t.Start))
            {
                stringBuilder.Replace(markdownToken.Line, tagsCollection[markdownToken].Line, markdownToken
                    .Start + delta, markdownToken.Length);
                delta += tagsCollection[markdownToken].Length - markdownToken.Length;
            }

            for (int i = 0; i <= stringBuilder.Length - 1; i++)
                if (stringBuilder[i] == '\\')
                    stringBuilder.Remove(i, 1);

            return stringBuilder.ToString();
        }
    }
}