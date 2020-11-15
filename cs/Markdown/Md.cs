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
                    switch (notFilteredDict[i])
                    {
                        case OpenItalicTag tag:
                            ignoreStrong = true;
                            break;
                        case CloseItalicTag tag:
                            ignoreStrong = false;
                            break;
                        case BoldTag tag:
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
            allTags.AddRange(ReadHeaderToken(line));
            allTags.AddRange(ReadAllItalicTokens(line));
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
                return new Tag[] {new OpenHeaderTag(0), new CloseHeaderTag(line.Length) };
            }
            return new Tag[0];
        }
        public static Tag[] ReadAllItalicTokens(string line)
        {
            var result = new List<Tag>();
            for (var index = 0; index < line.Length; index++)
            {
                if (ItalicTag.IsTokenStart(line, index))
                {
                    var endOfToken = GetEndOfItalicToken(line, index);
                    if (endOfToken == -1)
                    {
                        return result.ToArray();
                    }
                    result.Add(new OpenItalicTag(index));
                    result.Add(new CloseItalicTag(endOfToken));
                    index = endOfToken;
                }
            }

            return result.ToArray();
        }
        public static Tag[] ReadAllBoldTokens(string line)
        {
            var result = new List<Tag>();
            for (var index = 0; index < line.Length; index++)
            {
                if (BoldTag.IsTokenStart(line, index))
                {
                    var endOfToken = GetEndOfBoldToken(line, index);
                    if (endOfToken == -1)
                    {
                        return result.ToArray();
                    }
                    result.Add(new OpenBoldTag(index));
                    result.Add(new CloseBoldTag(endOfToken));
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
            return GetEndOfToken(line, index, BoldTag.IsTokenEnd, 2);
        }

        private static int GetEndOfItalicToken(string line, int index)
        {
            return GetEndOfToken(line, index, ItalicTag.IsTokenEnd, 2);
        }
    }
}