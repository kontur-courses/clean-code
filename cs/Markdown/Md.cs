using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Markdown
{
    static class Md
    {
        public static string Render(string markdownText)
        {
            var resultString = new StringBuilder();
            var lines = markdownText
                .Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];
                line = MarkdownToHTML.replaceMarkdown(line, "__", "<strong>", "</strong>");
                line = MarkdownToHTML.replaceMarkdown(line, "_", "<em>", "</em>");
                line = line.Replace("\\", "");
                resultString.Append(line);
                if (lineIndex < lines.Length - 1)
                    resultString.Append("\n");
            }
            return resultString.ToString();
        }

    }

}