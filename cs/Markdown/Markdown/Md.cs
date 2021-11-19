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
            //throw new NotImplementedException();
        }

        private string AnalyzeParagraph(string paragraph)
        {
            var resultParagraph = new StringBuilder();
            var lines = paragraph.Split(new char['\n'], StringSplitOptions.RemoveEmptyEntries);
            if (lines[0][0] == '#')
            {
                // ...
            }

            foreach (var line in lines)
            {
                /*
                var analyzer = new HtmlAnalyzer();
                var analyzedLine = analyzer.AnalyzeLine(line);
                */

                var analyzer = new HtmlTokenAnalyzer();
                var analyzedLine = analyzer.AnalyzeLine(line);


                resultParagraph.Append(analyzedLine);
            }

            return resultParagraph.ToString();
            //throw new NotImplementedException();
        }
    }
}
