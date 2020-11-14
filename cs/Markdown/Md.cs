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
            var result = new List<Tag>();
            for (var index = 0; index < line.Length; index++)
            {
                if (IsItalicTokenStarting(line, index))
                {
                    var endOfToken = GetEndOfItalicToken(line, index);
                    if (endOfToken == -1)
                    {
                        return result.ToArray();
                    }
                    result.Add(new Tag("<em>", index));
                    result.Add(new Tag("</em>", endOfToken));
                    index = endOfToken;
                }
            }

            return result.ToArray();
        }
        public static Tag[] ReadAllBoldTokens(string line)
        {
            throw new NotImplementedException();
        }

        private static int GetEndOfItalicToken(string line, int index)
        {
            index++;
            while (index < line.Length)
            {
                if (IsItalicTokenEnded(line, index))
                {
                    return index;
                }

                index += SkipNotStyleWords(line, index);
            }

            return -1;
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