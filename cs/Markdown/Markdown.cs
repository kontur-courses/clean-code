using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdownText)
        {
            var temp = new MarkdownTokenizer().SplitTextToTokens(markdownText);
            
            var htmlFormattedText = FormatMarkdownText(temp);
            
            return htmlFormattedText;
        }

        public string FormatMarkdownText(List<Token> textTokens)
        {
            var str = new StringBuilder();
            foreach (var token in textTokens)
            {
                if (token.Type != TagType.None)
                {
                    var (start, end) = MarkdownTagsLibrary.TagToHtmlTagAssociations[token.Type];
                    str.Append(start);
                    str.Append(token.Text);
                    str.Append(end);
                }
                else 
                    str.Append(token.Text);
            }

            return str.ToString();
        }
    }
}
