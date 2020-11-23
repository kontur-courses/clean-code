using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Markdown
    {
        private static readonly ISet<string> Markdowns = new SortedSet<string>(new[] {"#", "_", "__"});

        public static string Render(string markdownText)
        {
            var htmlText = markdownText;
            var allTags = MarkdownTextAnalyzer.GetAllTags(markdownText).OrderByDescending(tag => tag.Position);
            foreach (var tag in allTags) htmlText = HtmlCreator.AddHtmlTagToText(htmlText, tag);

            return GetHtmlTextWithoutEscapingSymbols(htmlText);
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