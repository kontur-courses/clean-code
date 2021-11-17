using System;
using System.Collections.Generic;
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

            //return result.ToString();
            throw new NotImplementedException();
        }

        private string AnalyzeParagraph(string paragraph)
        {
            var lines = paragraph.Split(new char['\n'], StringSplitOptions.RemoveEmptyEntries);
            if (lines[0][0] == '#')
            {
                // ...
            }

            foreach (var line in lines)
            {
                var analyzedLine = HtmlAnalyzer.AnalyzeLine(line);
                // ...
            }

            throw new NotImplementedException();
        }
    }
}
