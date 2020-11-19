using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TagTokensParser
    {
        public static IEnumerable<TagToken> ReadAllTagTokensFromLine(string line)
        {
            var openedTags = new Stack<(int startPosition, TagType type)>();

            for (var i = 0; i < line.Length; i++)
            {
                var type = GetTagType(line, i, openedTags);

                switch (type)
                {
                    case TagType.NonTag:
                        continue;
                    case TagType.Shield:
                    {
                        var followingTag = GetTagType(line, i + 1, openedTags);
                        if (followingTag is TagType.NonTag)
                            continue;
                        yield return new SingleTagToken(i, line.Length, TagType.Shield);
                        var shift = TagAnalyzer.GetSignLength(followingTag);
                        i += shift;
                        break;
                    }
                    case TagType.Bold:
                    case TagType.Italic:
                    {
                        if (TryGetTagTokenForPairedTag(line, i, type, openedTags, out var token))
                            yield return token;
                        break;
                    }
                    case TagType.Header:
                    {
                        if (TryGetTagTokenForSingleTag(line, i, type, out var token))
                            yield return token;
                        break;
                    }
                    case TagType.Link:
                    {
                        if (TryGetTokenForLink(line, i, out var token))
                        {
                            yield return token;
                            var shift = token.TagSignLength;
                            i += shift;
                        }

                        break;
                    }
                }

                i += TagAnalyzer.GetSignLength(type) - 1;
            }
        }

        public static IEnumerable<TagToken> GetCorrectTagTokens(string line)
        {
            var tokens = ReadAllTagTokensFromLine(line);
            foreach (var token in tokens.OfType<SingleTagToken>())
                yield return token;

            var pairedTokens = tokens.OfType<PairedTagToken>().ToList();
            foreach (var token in pairedTokens)
            {
                if (!HasCorrectValueLength(token)
                    || TagAnalyzer.IsCoverPartOfWord(line, token) && !TagAnalyzer.IsTagInSameWord(line, token)
                    || TagAnalyzer.IsTagInsideWordWithDigits(line, token)
                    || HasIncorrectIntersection(pairedTokens, token)
                    || HasIncorrectNesting(pairedTokens, token))
                    continue;

                yield return token;
            }
        }

        private static bool TryGetTokenForLink(string line, int startIndex, out LinkTagToken token)
        {
            token = null;
            if (!TryGetLinkText(line, startIndex, out var linkText, out var linkTextCloserIndex))
                return false;

            var urlOpener = linkTextCloserIndex + 1;
            if (!TryGetLinkUrl(line, urlOpener, out var linkUrl, out var urlCloserIndex))
                return false;

            token = new LinkTagToken(startIndex, urlCloserIndex, TagType.Link, linkText, linkUrl);
            return true;
        }

        private static bool TryGetLinkText(string line, int startIndex, out string text, out int textCloserIndex)
        {
            text = string.Empty;
            textCloserIndex = startIndex;
            var linkTextCloserIndex = line.IndexOf(']', startIndex);
            if (linkTextCloserIndex == -1)
                return false;

            var linkTextLength = linkTextCloserIndex - startIndex;
            text = line.Substring(startIndex + 1, linkTextLength - 1);
            textCloserIndex = linkTextCloserIndex;
            return true;
        }

        private static bool TryGetLinkUrl(string line, int urlStartIndex, out string url, out int urlCloserIndex)
        {
            url = string.Empty;
            urlCloserIndex = urlStartIndex;
            if (line[urlStartIndex] != '(')
                return false;

            var urlCloser = line.IndexOf(')', urlStartIndex);
            if (urlCloser == -1)
                return false;

            url = line.Substring(urlStartIndex + 1, urlCloser - urlStartIndex - 1);
            if (url.Contains(' '))
                return false;
            urlCloserIndex = urlCloser;

            return true;
        }

        private static bool TryGetTagTokenForPairedTag(string line, int tagIndex, TagType type, Stack<(int startPosition, TagType type)> openedTags, out PairedTagToken token)
        {
            token = null;
            var signLength = TagAnalyzer.GetSignLength(type);

            if (openedTags.Any() && openedTags.Peek().type == type)
            {
                if (tagIndex >= 1 && char.IsWhiteSpace(line[tagIndex - 1]))
                    return false;

                var opener = openedTags.Pop();
                token = new PairedTagToken(opener.startPosition, tagIndex, type);
                return true;
            }

            if (IsPossibleToOpenTag(tagIndex, signLength, line))
                openedTags.Push((tagIndex, type));

            if (IsCloser(line, tagIndex, type) && openedTags.Any())
                openedTags.Pop();

            return false;
        }

        private static bool IsCloser(string line, int tagIndex, TagType type) =>
            tagIndex + TagAnalyzer.GetSignLength(type) < line.Length && char.IsWhiteSpace(line[tagIndex + TagAnalyzer.GetSignLength(type)]);

        private static bool IsPossibleToOpenTag(int tagIndex, int signLength, string line)
        {
            return tagIndex + signLength < line.Length && !char.IsWhiteSpace(line[tagIndex + signLength]);
        }

        private static bool TryGetTagTokenForSingleTag(string line, int tagStartIndex, TagType type, out SingleTagToken token)
        {
            token = null;
            if (tagStartIndex != 0)
                return false;

            token = new SingleTagToken(tagStartIndex, line.Length, type);
            return true;
        }

        private static TagType GetTagType(string line, int index, Stack<(int startPosition, TagType type)> openedTags)
        {
            switch (line[index])
            {
                case '\\':
                    return index + 1 < line.Length && GetTagType(line, index + 1, openedTags) != TagType.NonTag
                        ? TagType.Shield
                        : TagType.NonTag;
                case '#': return index + 1 < line.Length && line[index + 1] == ' ' ? TagType.Header : TagType.NonTag;
                case '_':
                {
                    if (openedTags.Any() && HasOpenedItalicTag(line, index, openedTags))
                        return TagType.Italic;
                    return index + 1 < line.Length && line[index + 1] == '_' ? TagType.Bold : TagType.Italic;
                }
                case '[': return TagType.Link;
                default: return TagType.NonTag;
            }
        }

        private static bool HasOpenedItalicTag(string line, int index, Stack<(int startPosition, TagType type)> openedTags) =>
            openedTags.Peek().type is TagType.Italic
            && index + 2 < line.Length
            && line[index + 1] == '_' && line[index + 2] == '_';

        private static bool HasCorrectValueLength(TagToken token)
            => token.ValueLength > 0 || token.Type == TagType.Shield || token is LinkTagToken;

        private static bool HasIncorrectIntersection(IEnumerable<PairedTagToken> pairedTokens, PairedTagToken token) =>
            pairedTokens.Where(x => x != token)
                .Any(x => !TagAnalyzer.IsCorrectIntersection(token, x) && !token.IsInsideOf(x));

        private static bool HasIncorrectNesting(IEnumerable<PairedTagToken> pairedTokens, PairedTagToken token) =>
            pairedTokens.Where(x => x != token).Any(x => !TagAnalyzer.IsCorrectNesting(x, token));
    }
}