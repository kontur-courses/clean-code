using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var stringAccumulator = new StringBuilder();
            foreach (var line in text.Split('\n'))
            {
                stringAccumulator.Append(DeShield(RenderOneString(line)));
            }

            return stringAccumulator.ToString();
        }

        private string DeShield(string line)
        {
            var resultLine = new StringBuilder();
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] != '\\' || i == line.Length - 1 || (line[i + 1] != '_' && line[i + 1] != '#'))
                {
                    resultLine.Append(line[i]);
                }
            }

            return resultLine.ToString();
        }

        private string RenderOneString(string line)
        {
            var dict = 
                MarkdownParser.FilterTags(MarkdownParser.ReadAllTags(line), line.Length);
            var result = new StringBuilder();
            var i = 0;

            while (i < line.Length)
            {
                if (dict.ContainsKey(i))
                {
                    result.Append(dict[i].Value);
                    i += dict[i].Length;
                }
                else
                {
                    result.Append(line[i]);
                    i++;
                }
            }

            if (dict.ContainsKey(line.Length))
            {
                result.Append(dict[line.Length].Value);
            }
            return result.ToString();
        }
    }

    internal static class MarkdownParser
    {
        public static Dictionary<int, Tag> FilterTags(Dictionary<int, Tag> notFilteredDict, int length)
        {
            var ignoreStrong = false;
            for (var i = 0; i < length + 1; i++)
            {
                if (notFilteredDict.ContainsKey(i))
                {
                    switch (notFilteredDict[i].Value)
                    {
                        case "<em>":
                            ignoreStrong = true;
                            break;
                        case "</em>":
                            ignoreStrong = false;
                            break;
                        case "<strong>": case "</strong>":
                            if (ignoreStrong)
                            {
                                notFilteredDict.Remove(i);
                            }
                            break;
                    }
                }
            }
            return notFilteredDict;
        }
        public static Dictionary<int, Tag> ReadAllTags(string line)
        {
            var allTags = new List<Tag>();
            allTags.AddRange(MarkdownParser.ReadHeaderToken(line));
            allTags.AddRange(MarkdownParser.ReadAllItalicTokens(line));
            allTags.AddRange(ReadAllBoldTokens(line));
            var dict = new Dictionary<int, Tag>();
            foreach (var tag in allTags)
            {
                dict[tag.Index] = tag;
            }

            return dict;
        }
        public static Tag[] ReadHeaderToken(string line)
        {
            if (line[0] == '#')
            {
                return new [] {new Tag("<h1>", 0, 1), new Tag("</h1>", line.Length, 0)};
            }
            return new Tag[0];
        }
        public static Tag[] ReadAllItalicTokens(string line)
        {
            return ReadAllTokensByRules(line, IsItalicTokenStart, GetEndOfItalicToken,
                "<em>", "</em>", 1);
        }
        public static Tag[] ReadAllBoldTokens(string line)
        {
            return ReadAllTokensByRules(line, IsBoldTokenStart, GetEndOfBoldToken,
                "<strong>", "</strong>", 2);
        }
        private static Tag[] ReadAllTokensByRules(string line, Func<string, int, bool> isTokenStart,
            Func<string, int, int> getTokenEnd, string startValue, string endValue, int length)
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
                    result.Add(new Tag(startValue, index, length));
                    result.Add(new Tag(endValue, endOfToken, length));
                    index = endOfToken;
                }
            }

            return result.ToArray();
        }

        private static int GetEndOfToken(string line, int index, Func<string, int, bool> isTokenEnd, int length)
        {
            index += length;
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
            return GetEndOfToken(line, index, IsBoldTokenEnd, 2);
        }

        private static int GetEndOfItalicToken(string line, int index)
        {
            return GetEndOfToken(line, index, IsItalicTokenEnd, 2);
        }

        private static bool IsItalicTokenEnd(string line, int index)
        {
            return line[index] == '_' && line[index - 1] != ' '
                                      && line[index - 1] != '\\'
                                      && (index == line.Length - 1 || line[index + 1] != '_' || line[index - 1] == '\\')
                                      && line[index - 1] != '_';
        }

        private static bool IsItalicTokenStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex != line.Length - 1
                   && (line[startIndex + 1] != '_' || startIndex > 0 && line[startIndex - 1] == '\\')
                   && !Char.IsDigit(line[startIndex + 1])
                   && line[startIndex + 1] != ' '
                   && (startIndex == 0 || line[startIndex - 1] != '_' && line[startIndex - 1] != '\\') ;
        }
        private static bool IsBoldTokenStart(string line, int startIndex)
        {
            return line[startIndex] == '_'
                   && startIndex <= line.Length - 2
                   && line[startIndex + 1] == '_'
                   && (startIndex == line.Length - 2 || line[startIndex + 2] != ' ')
                   && (startIndex == 0 || line[startIndex - 1] != '\\');
        }

        private static bool IsBoldTokenEnd(string line, int endIndex)
        {
            return line[endIndex] == '_' && line[endIndex + 1] == '_' && 
                   (endIndex < 1 || line[endIndex - 1] != ' ');
        }
    }
}