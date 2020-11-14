using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string[] text)
        {
            throw new NotImplementedException();
        }
    }

    internal static class MarkdownParser
    {
        public static Tag[] ReadAllItalicTokens(string line)
        {
            throw new NotImplementedException();
        }
        public static Tag[] ReadAllBoldTokens(string line)
        {
            throw new NotImplementedException();
        }

        private static bool IsItalicTokenEnded(string line, int index)
        {
            return line[index] == '_' && line[index - 1] != ' ';
        }

        private static bool IsItalicTokenStarting(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex != line.Length - 1
                   && line[startIndex + 1] != '_'
                   && !Char.IsDigit(line[startIndex + 1])
                   && line[startIndex + 1] != ' ';
        }
        private static bool IsBoldTokenStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex <= line.Length - 4
                   && line[startIndex + 1] == '_'
                   && line[startIndex + 2] != ' ';
        }

        private static bool IsBoldTokenEnd(string line, int endIndex)
        {
            return line[endIndex - 1] == '_' && line[endIndex] == '_' && line[endIndex - 2] != ' ';
        }

        public static int SkipNotStyleWords(string line, int startIndex)
        {
            for (var i = startIndex; i < line.Length; i++)
            {
                if (line[i] == '_')
                {
                    return i - startIndex;
                }
            }
            return line.Length - startIndex;
        }
    } 
}