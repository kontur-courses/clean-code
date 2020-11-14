using System;
using System.Collections.Generic;

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
        public static Tag[] ReadHeaderToken(string line)
        {
            if (line[0] == '#')
            {
                return new [] {new Tag("<h1>", 0), new Tag("</h1>", line.Length - 1)};
            }
            return new Tag[0];
        }
        public static Tag[] ReadAllItalicTokens(string line)
        {
            return ReadAllTokens(line, IsItalicTokenStart, GetEndOfItalicToken, "<em>", "</em>");
        }
        public static Tag[] ReadAllBoldTokens(string line)
        {
            return ReadAllTokens(line, IsBoldTokenStart, GetEndOfBoldToken, "<strong>", "</strong>");
        }
        private static Tag[] ReadAllTokens(string line, Func<string, int, bool> isTokenStart,
            Func<string, int, int> getTokenEnd, string startValue, string endValue)
        {
            
            var result = new List<Tag>();
            for (var index = 0; index < line.Length; index++)
            {
                if (isTokenStart(line, index))
                {
                    var endOfToken = getTokenEnd(line, index);
                    if (endOfToken == -1)
                    {
                        return result.ToArray();
                    }
                    result.Add(new Tag(startValue, index));
                    result.Add(new Tag(endValue, endOfToken));
                    index = endOfToken;
                }
            }

            return result.ToArray();
        }

        private static int GetEndOfToken(string line, int index, Func<string, int, bool> isTokenEnd)
        {
            index++;
            while (index < line.Length)
            {
                if (isTokenEnd(line, index))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        private static int GetEndOfBoldToken(string line, int index)
        {
            return GetEndOfToken(line, index, IsBoldTokenEnd);
        }

        private static int GetEndOfItalicToken(string line, int index)
        {
            return GetEndOfToken(line, index, IsItalicTokenEnd);
        }

        private static bool IsItalicTokenEnd(string line, int index)
        {
            return line[index] == '_' && line[index - 1] != ' ';
        }

        private static bool IsItalicTokenStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex != line.Length - 1
                   && line[startIndex + 1] != '_'
                   && !Char.IsDigit(line[startIndex + 1])
                   && line[startIndex + 1] != ' ';
        }
        private static bool IsBoldTokenStart(string line, int startIndex)
        {
            return startIndex > 0 && line[startIndex - 1] == '_'
                   && startIndex <= line.Length - 3
                   && line[startIndex] == '_'
                   && line[startIndex + 1] != ' ';
        }

        private static bool IsBoldTokenEnd(string line, int endIndex)
        {
            return line[endIndex - 1] == '_' && line[endIndex] == '_' && 
                   (endIndex < 2 || line[endIndex - 2] != ' ');
        }
    }
}