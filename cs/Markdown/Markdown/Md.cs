using System;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdownText)
        {
            var result = new StringBuilder();
            var paragraphs = markdownText.Split(new char['\r'], StringSplitOptions.RemoveEmptyEntries);
            for (int i=0; i< paragraphs.Length; i++)
            {
                var analyzedParagraph = AnalyzeParagraph(paragraphs[i]);
                result.Append(analyzedParagraph);
                if (i != paragraphs.Length - 1)
                    result.Append('\r');
            }
            return result.ToString();
        }

        private static string AnalyzeParagraph(string paragraph)
        {
            var resultParagraph = new StringBuilder();
            var lines = paragraph.Split(new char['\n'], StringSplitOptions.RemoveEmptyEntries);
            var isHeader = false;
            if (IsThisLineHasHeader(lines[0]))
            {
                isHeader = true;
                lines[0] = lines[0].Substring(2);
            }

            for (int i=0; i < lines.Length; i++)
            {
                var analyzer = new HtmlTokenAnalyzer();
                var analyzedLine = analyzer.AnalyzeLine(lines[i]);
                resultParagraph.Append(analyzedLine);
                if (i != lines.Length - 1)
                    resultParagraph.Append('\n');
            }

            if (isHeader)
                return "<h1>" + resultParagraph + "</h1>";
            return resultParagraph.ToString();
        }

        private static bool IsThisLineHasHeader(string line)
        {
            return (line[0] == '#');
        }
    }
}
