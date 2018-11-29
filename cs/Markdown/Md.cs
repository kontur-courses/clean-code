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
                line = ReplaceMarkdown(line, "__", "<strong>", "</strong>");
                line = ReplaceMarkdown(line, "_", "<em>", "</em>");
                line = line.Replace("\\", "");
                resultString.Append(line);
                if (lineIndex < lines.Length - 1)
                    resultString.Append("\n");
            }
            return resultString.ToString();
        }

        private static string ReplaceMarkdown(string line, string mdTag,
            string htmlStartTag, string htmlEndTag)
        {
            while (true)
            {
                var firstMdTag = GetEntry(line, mdTag, -1, IsValidStartEntry);
                var secondMdTag = GetEntry(line, mdTag, firstMdTag, IsValidEndEntry);
                if (secondMdTag == -1 || firstMdTag == -1)
                    return line;
                else
                {
                    line = ChangeMdToHTML(line, htmlEndTag, secondMdTag, secondMdTag + mdTag.Length - 1);
                    line = ChangeMdToHTML(line, htmlStartTag, firstMdTag, firstMdTag + mdTag.Length - 1);
                }
            }
        }

        private static string ChangeMdToHTML(string line,
            string htmlTag, int entryStartPosition, int entryEndPosition)
        {
            var resultString = new StringBuilder();
            resultString.Append(line.Substring(0, entryStartPosition));
            resultString.Append(htmlTag);
            if (entryEndPosition + 1 < line.Length)
                resultString.Append(line.Substring(entryEndPosition + 1));
            return resultString.ToString();
        }

        private static int GetEntry(string line, string mdTag, int findFromPosition, Func<string, int, int, bool> isValid)
        {
            var currentPosition = findFromPosition + 1;
            while (true)
            {
                var entry = line.IndexOf(mdTag, currentPosition);
                var entryEnd = entry + mdTag.Length - 1;
                if (entry == -1) return -1;
                if (isValid(line, entry, entryEnd))
                    return entry;
                else
                    currentPosition = entryEnd + 1;
            }
        }

        

}