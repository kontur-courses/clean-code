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
            foreach (var paragraph in paragraphs)
            {
                string analyzedParagraph = AnalyzeParagraph(paragraph);
                result.Append(analyzedParagraph);
            }
            Console.WriteLine();
            return result.ToString();
        }

        private string AnalyzeParagraph(string paragraph)
        {
            var resultParagraph = new StringBuilder();
            var lines = paragraph.Split(new char['\n'], StringSplitOptions.RemoveEmptyEntries);
            var isHeader = false;
            if (lines[0][0] == '#')
            {
                isHeader = true;
                lines[0] = lines[0].Substring(2);
                // ...
            }

            foreach (var line in lines)
            {
                var analyzer = new HtmlTokenAnalyzer();
                var analyzedLine = analyzer.AnalyzeLine(line);
                resultParagraph.Append(analyzedLine);
            }

            if (isHeader)
                return "<h1>" + resultParagraph + "</h1>";
            return resultParagraph.ToString();
        }
    }
}
