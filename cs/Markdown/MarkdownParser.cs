using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class MarkdownParser
    {
        public static Dictionary<int, Tag> FilterTags(Tag[] tags, int length)
        {
            var notFilteredDict = GetTagsDictionary(tags);
            var ignoreStrong = false;
            for (var i = 0; i < length + 1; i++)
            {
                if (notFilteredDict.ContainsKey(i))
                {
                    switch (notFilteredDict[i])
                    {
                        case OpenItalicTag _:
                            ignoreStrong = true;
                            break;
                        case CloseItalicTag _:
                            ignoreStrong = false;
                            break;
                        case BoldTag _:
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
        public static Tag[] ReadAllTags(string line)
        {
            var allTags = new List<Tag>();
            allTags.AddRange(ReadHeaderToken(line));
            allTags.AddRange(ReadAllItalicTokens(line));
            allTags.AddRange(ReadAllBoldTokens(line));

            return allTags.ToArray();
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

        private static Dictionary<int, Tag> GetTagsDictionary(Tag[] tags)
        {
            var result = new Dictionary<int, Tag>();
            foreach (var tag in tags)
            {
                result[tag.Index] = tag;
            }

            return result;
            
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