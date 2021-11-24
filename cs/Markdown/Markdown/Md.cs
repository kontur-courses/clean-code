﻿using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdownText)
        {
            var htmlLinesBuilder = new StringBuilder();
            var lines = markdownText.Split(new[] {'\r','\n'});
            if (IsThisLineHasHeader(lines[0]))
            {
                lines[lines.Length-1] = lines[lines.Length - 1]  + "#";
                lines[0] = "#" + lines[0].Substring(2);
            }

            for (int i = 0; i < lines.Length; i++)
            {
                var analyzer = new HtmlTokenAnalyzer();
                var analyzedLine = analyzer.AnalyzeLine(lines[i]);
                htmlLinesBuilder.Append(analyzedLine);
                if (i != lines.Length - 1)
                    htmlLinesBuilder.Append('\n');
            }
            return htmlLinesBuilder.ToString();
        }

        private static bool IsThisLineHasHeader(string line)
        {
            return line[0] == '#';
        }
    }
}
