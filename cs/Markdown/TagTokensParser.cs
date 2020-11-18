using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TagTokensParser
    {
        public static List<TagToken> ReadTagsFromLine(string line)
        {
            var openedTags = new Stack<(int startPosition, TagType type)>();
            var tokens = new List<TagToken>();

            for (var i = 0; i < line.Length; i++)
            {
                var type = GetTagType(line, i);
                if (type == TagType.NonTag)
                    continue;

                switch (type)
                {
                    case TagType.Shield:
                    {
                        var followingTag = GetTagType(line, i + 1);
                        if (followingTag is TagType.NonTag)
                            continue;
                        tokens.Add(new TagToken(i, line.Length, TagType.Shield));
                        var shift = TagAnalyzer.GetSignLength(followingTag);
                        i += shift;
                        break;
                    }
                    case TagType.Bold:
                    case TagType.Italic:
                    {
                        if (TryGetTagTokenForPairedTag(line, i, type, openedTags, out var token))
                            tokens.Add(token);
                        break;
                    }
                    case TagType.Header:
                    {
                        if (TryGetTagTokenForSingleTag(line, i, type, out var token))
                            tokens.Add(token);
                        break;
                    }
                }

                i += TagAnalyzer.GetSignLength(type) - 1;
            }

            return tokens;
        }

        private static bool TryGetTagTokenForPairedTag(string line, int tagIndex, TagType type, Stack<(int startPosition, TagType type)> openedTags, out TagToken token)
        {
            token = null;
            var signLength = TagAnalyzer.GetSignLength(type);

            if (openedTags.Any() && openedTags.Peek().type == type)
            {
                if (tagIndex >= 1 && char.IsWhiteSpace(line[tagIndex - 1]))
                    return false;

                var opener = openedTags.Pop();
                token = new TagToken(opener.startPosition, tagIndex, type);
                return true;
            }

            if (IsPossibleToOpenTag(tagIndex, signLength, line))
                openedTags.Push((tagIndex, type));

            return false;
        }

        private static bool IsPossibleToOpenTag(int tagIndex, int signLength, string line)
        {
            return tagIndex + signLength < line.Length && !char.IsWhiteSpace(line[tagIndex + signLength]);
        }

        private static bool TryGetTagTokenForSingleTag(string line, int tagStartIndex, TagType type, out TagToken token)
        {
            token = null;
            if (tagStartIndex != 0)
                return false;

            token = new TagToken(tagStartIndex, line.Length, type);
            return true;
        }

        private static TagType GetTagType(string line, int index)
        {
            switch (line[index])
            {
                case '\\':
                    return index + 1 < line.Length && GetTagType(line, index + 1) != TagType.NonTag
                        ? TagType.Shield
                        : TagType.NonTag;
                case '#': return index + 1 < line.Length && line[index + 1] == ' ' ? TagType.Header : TagType.NonTag;
                case '_': return index + 1 < line.Length && line[index + 1] == '_' ? TagType.Bold : TagType.Italic;
                default: return TagType.NonTag;
            }
        }

        public static void RemoveIncorrectTokens(string line, List<TagToken> tokens)
        {
            tokens.RemoveAll(x =>
                x.ValueLength == 0 && x.Type != TagType.Shield
                || TagAnalyzer.IsCoverPartOfWord(line, x) && !TagAnalyzer.IsTagInSameWord(line, x)
                || TagAnalyzer.IsTagInsideWordWithDigits(line, x));
            RemoveIncorrectIntersections(tokens);
            RemoveIncorrectNestings(tokens);
        }

        private static void RemoveIncorrectIntersections(List<TagToken> tags)
        {
            var toRemove = new List<TagToken>();

            foreach (var tag in tags)
            {
                var wrongIntersections = tags.Where(x => x != tag && !TagAnalyzer.IsCorrectIntersection(tag, x) && !tag.IsInsideOf(x)).ToList();
                toRemove.AddRange(wrongIntersections);
                if (wrongIntersections.Any())
                    toRemove.Add(tag);
            }

            tags.RemoveAll(x => toRemove.Contains(x));
        }

        private static void RemoveIncorrectNestings(List<TagToken> tokens)
        {
            var toRemove = tokens.Where(token => tokens.Any(x => !TagAnalyzer.IsCorrectNesting(x, token))).ToList();

            tokens.RemoveAll(x => toRemove.Contains(x));
        }
    }
}