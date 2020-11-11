using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Markdown
    {
        private static readonly string[] Markdowns = {"#", "_", "__"};

        public static string Render(string markdownText)
        {
            var paragraphs = markdownText.Split('\n');
            var htmlText = new StringBuilder();
            foreach (var paragraph in paragraphs)
            {
                var tokens = MarkdownTextAnalyzer.GetTokens(paragraph);
                var htmlParagraph = paragraph;
                tokens = tokens.OrderBy(token => token.StartPosition);
                foreach (var token in tokens)
                    htmlParagraph = HtmlCreator.AddHtmlTagToText(htmlParagraph, token);
                htmlText.Append(htmlParagraph);
            }

            return GetHtmlTextWithoutEscapingSymbols(htmlText.ToString());
        }

        private static string GetHtmlTextWithoutEscapingSymbols(string htmlText)
        {
            var result = new StringBuilder();
            for (var i = 0; i < htmlText.Length; i++)
                if (i < htmlText.Length - 1 && htmlText[i] == '\\' &&
                    (Markdowns.Contains(htmlText[i + 1].ToString()) || htmlText[i + 1] == '\\'))
                {
                    result.Append(htmlText[i + 1]);
                    i++;
                }
                else
                {
                    result.Append(htmlText[i]);
                }

            return result.ToString();
        }
    }
}