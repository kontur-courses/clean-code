using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Infrastructure.Blocks;
using Markdown.Infrastructure.Parsers.Tags;

namespace Markdown.Infrastructure.Parsers
{
    public class MarkdownParser : BlockParser
    {
        private readonly int maxPictureDescriptionLength = 20;

        /// <summary>
        ///     Validate interaction of tags according to rules
        /// </summary>
        protected override IEnumerable<TagInfo> GetValidTags(IEnumerable<TagInfo> tagInfos, string text)
        {
            tagInfos = FilterEscaped(tagInfos);
            tagInfos = FilterIntersections(tagInfos, text).OrderBy(tagInfo => tagInfo.Offset);
            tagInfos = FilterDoubleInSingle(tagInfos);
            tagInfos = FilterEmptyTags(tagInfos, text);

            return tagInfos;
        }

        private IEnumerable<TagInfo> FilterDoubleInSingle(IEnumerable<TagInfo> tagInfos)
        {
            var isUnderscoreOpen = false;
            foreach (var tagInfo in tagInfos)
            {
                if (tagInfo.Tag.Style == Style.Angled)
                    isUnderscoreOpen = !isUnderscoreOpen;

                if (tagInfo.Tag.Style == Style.Bold && isUnderscoreOpen)
                    continue;

                yield return tagInfo;
            }
        }

        private IEnumerable<TagInfo> FilterEmptyTags(IEnumerable<TagInfo> tagInfos, string text)
        {
            var toSkip = new List<TagInfo>();

            var enumerable = tagInfos.ToList();
            foreach (var (previous, current) in enumerable.Zip(enumerable.Skip(1)))
                if (current.Closes(previous, text) && current.Follows(previous))
                {
                    toSkip.Add(previous);
                    toSkip.Add(current);
                }

            return enumerable.Where(info => !toSkip.Contains(info));
        }

        private IEnumerable<TagInfo> FilterEscaped(IEnumerable<TagInfo> tagInfos)
        {
            using var enumerator = tagInfos.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var tagInfo = enumerator.Current;
                if (tagInfo.Tag.Style == Style.Escape)
                {
                    enumerator.MoveNext();
                    if (enumerator.Current.Follows(tagInfo))
                        yield return tagInfo;
                }
                else
                {
                    yield return tagInfo;
                }
            }
        }

        private IEnumerable<TagInfo> FilterIntersections(IEnumerable<TagInfo> tagInfos, string text)
        {
            var unclosed = new Stack<TagInfo>();
            foreach (var tagToCheck in tagInfos)
                if (unclosed.TryPeek(out var tagToClose) && tagToCheck.Closes(tagToClose, text))
                {
                    yield return unclosed.Pop();
                    yield return tagToCheck;
                }
                else
                {
                    unclosed.Push(tagToCheck);
                }
        }

        /// <summary>
        ///     Parse tags according to documented rules
        /// </summary>
        protected override IEnumerable<TagInfo> ParseTags(string text)
        {
            var processed = 0;
            while (processed < text.Length)
            {
                var tagInfo = ParseTag(ref text, processed);
                var shift = 0;
                if (tagInfo != null)
                {
                    shift = tagInfo.Length;
                    yield return tagInfo;
                }

                processed += Math.Max(shift, 1);
            }

            yield return GetEmptyEnterTag(text.Length);
        }

        private TagInfo GetEmptyEnterTag(int offset)
        {
            return new TagInfo(offset, 0, Style.NewLine);
        }

        private TagInfo ParseTag(ref string text, int offset)
        {
            switch (text[offset])
            {
                case '_':
                    return ParseUnderscore(ref text, offset);
                case '\\':
                    return ParseEscapeSymbol(ref text, offset);
                case '#':
                    return ParseHeader(ref text, offset);
                case '!':
                    return ParsePicture(ref text, offset);
                case '[':
                    return ParseLink(ref text, offset);
                case ')':
                    return ParseMediaEnd(ref text, offset);
                default:
                    return TryParseNewLine(ref text, offset, out var tagInfo)
                        ? tagInfo
                        : null;
            }
        }

        private TagInfo ParseMediaEnd(ref string text, int offset)
        {
            return new TagInfo(offset, 1, Style.Media, true, false);
        }

        private TagInfo ParseLink(ref string text, in int offset)
        {
            Tag CreateTag(string link) { return new LinkTag(link); }
            return ParseMedia(ref text, offset, CreateTag, 0);
        }

        private TagInfo ParsePicture(ref string text, int offset)
        {
            Tag CreateTag(string description) { return new PictureTag(description); }
            return ParseMedia(ref text, offset, CreateTag, 1);
        }

        private TagInfo ParseMedia(ref string text, int offset, CreateTag createTag, int processed)
        {
            var payload = ParsePayload(ref text, offset, processed);
            if (payload is null)
                return null;
            processed += payload.Length + 2;

            if (CharIs('(', ref text, offset + processed))
            {
                processed++;
                return new TagInfo(offset, processed, createTag(payload), false);
            }

            return null;
        }

        private string ParsePayload(ref string text, int offset, int processed)
        {
            if (CharIs('[', ref text, offset + processed))
            {
                processed++;
                for (var payloadLength = 0; payloadLength < maxPictureDescriptionLength; payloadLength++)
                    if (CharIs(']', ref text, offset + payloadLength + processed))
                        return text.Substring(offset + processed, payloadLength);
            }

            return null;
        }

        public static bool CharIs(char possibleChar, ref string text, int offset)
        {
            return IsInBounds(ref text, offset) && text[offset] == possibleChar;
        }

        private static bool CharIsNumber(ref string text, int offset)
        {
            for (var i = 0; i < 10; i++)
                if (CharIs(i.ToString()[0], ref text, offset))
                    return true;

            return false;
        }

        public static bool CharIsTextChar(ref string text, int offset)
        {
            return !CharIsWhiteSpace(ref text, offset)
                   && !CharIsNumber(ref text, offset);
        }

        public static bool CharIsWhiteSpace(ref string text, int offset)
        {
            return CharIs(' ', ref text, offset)
                   || CharIs('\t', ref text, offset);
        }

        public static bool IsInBounds(ref string text, int offset)
        {
            return offset >= 0 && offset < text.Length;
        }

        public static bool CharBetweenTags(char c, ref string text, TagInfo start, TagInfo end)
        {
            for (var offset = start.Offset + start.Length; offset < end.Offset; offset++)
                if (CharIs(c, ref text, offset))
                    return true;

            return false;
        }

        public static bool WhiteSpaceCharBetweenTags(ref string text, TagInfo start, TagInfo end)
        {
            return CharBetweenTags(' ', ref text, start, end)
                   || CharBetweenTags('\t', ref text, start, end);
        }

        private TagInfo ParseUnderscore(ref string text, int offset)
        {
            return CharIs('_', ref text, offset + 1)
                ? ParseDoubleUnderscore(ref text, offset)
                : ParseSingleUnderscore(ref text, offset);
        }

        private TagInfo ParseDoubleUnderscore(ref string text, int offset)
        {
            if (CharIs(' ', ref text, offset + 1) || offset == text.Length - 1)
                return new TagInfo(offset, 2, Style.Bold, true, false);

            if (CharIs(' ', ref text, offset - 1) || offset == 0)
                return new TagInfo(offset, 2, Style.Bold, false);

            return new TagInfo(offset, 2, Style.Bold);
        }

        private TagInfo ParseSingleUnderscore(ref string text, int offset)
        {
            if (CharIsNumber(ref text, offset + 1)
                && !CharIsWhiteSpace(ref text, offset - 1))
                return null;

            if (CharIs(' ', ref text, offset + 1) || offset == text.Length - 1)
                return new TagInfo(offset, 1, Style.Angled, true, false);

            if (CharIs(' ', ref text, offset - 1) || offset == 0)
                return new TagInfo(offset, 1, Style.Angled, false);

            return new TagInfo(offset, 1, Style.Angled);
        }

        private TagInfo ParseEscapeSymbol(ref string text, int offset)
        {
            return CharIs('\\', ref text, offset + 1)
                ? null
                : new TagInfo(offset, 1, Style.Escape);
        }

        private TagInfo ParseHeader(ref string text, int offset)
        {
            return CharIs(' ', ref text, offset + 1)
                ? new TagInfo(offset, 2, Style.Header)
                : null;
        }

        private bool TryParseNewLine(ref string text, int offset, out TagInfo tagInfo)
        {
            var newLine = Environment.NewLine;
            var substring = text.Substring(offset, newLine.Length);
            if (substring == newLine)
            {
                tagInfo = new TagInfo(offset, 2, Style.NewLine);
                return true;
            }

            tagInfo = null;
            return false;
        }

        private delegate Tag CreateTag(string payload);
    }
}