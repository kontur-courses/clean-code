using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class MarkdownToHTML
    {
        public static string replaceMarkdown(string line, string oldString,
            string newStartString, string newEndString)
        {
            while (true)
            {
                var start = getStartEntry(line, oldString);
                var end = getEndEntry(line, oldString, start);
                if (end == -1 || start == -1)
                    return line;
                else
                {
                    line = changeMdToHTML(line, newEndString, end, end + oldString.Length - 1);
                    line = changeMdToHTML(line, newStartString, start, start + oldString.Length - 1);
                }
            }
        }

        private static string changeMdToHTML(string line,
            string newString, int startEntry, int finishEntry)
        {
            var resultString = new StringBuilder();
            resultString.Append(line.Substring(0, startEntry));
            resultString.Append(newString);
            if (finishEntry + 1 < line.Length)
                resultString.Append(line.Substring(finishEntry + 1));
            return resultString.ToString();
        }

        private static int getStartEntry(string line, string substring)
        {
            var currentPos = 0;
            while (true)
            {
                var entry = getNextEntry(line, currentPos, substring);
                var entryEnd = entry + substring.Length - 1;
                if (entry == -1) return -1;
                if (isValidStartEntry(line, entry, entryEnd))
                    return entry;
                else
                    currentPos = entryEnd + 1;
            }
        }

        private static int getEndEntry(string line, string substring, int start)
        {
            var currentPos = start + 1;
            while (true)
            {
                var entry = getNextEntry(line, currentPos, substring);
                var entryEnd = entry + substring.Length - 1;
                if (entry == -1) return -1;
                if (isValidEndEntry(line, entry, entryEnd))
                    return entry;
                else
                    currentPos = entryEnd + 1;
            }
        }

        private static int getNextEntry(string line, int start, string substring)
        {
            return line.IndexOf(substring, start);
        }

        private static bool isValidStartEntry(string line, int entryStart, int entryEnd)
        {
            if (line.Length <= entryEnd + 1) return false;
            if (line[entryEnd + 1] == ' ') return false;
            if (!isValidEntry(line, entryStart, entryEnd)) return false;
            return true;
        }

        private static bool isValidEndEntry(string line, int entryStart, int entryEnd)
        {
            if (entryStart == 0) return false;
            if (line[entryStart - 1] == ' ') return false;
            if (!isValidEntry(line, entryStart, entryEnd)) return false;
            return true;
        }

        private static bool isValidEntry(string line, int substringStart, int substringEnd)
        {

            if (substringStart - 1 >= 0 &&
                char.IsDigit(line[substringStart - 1]) ||
                substringEnd + 1 < line.Length &&
                char.IsDigit(line[substringEnd + 1])) return false;

            if (substringStart - 1 >= 0 &&
                line[substringStart - 1] == '_' ||
                substringEnd + 1 < line.Length &&
                line[substringEnd + 1] == '_') return false;

            if (substringStart - 1 >= 0 &&
                line[substringStart - 1] == '\\') return false;
            return true;
        }
    }
}
