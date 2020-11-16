using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;
using Markdown.Infrastructure.Parsers.Tags;

namespace Markdown.Infrastructure.Parsers.Markdown
{
    public class MarkdownParser : IBlockParser
    {
        private readonly int maxPictureDescriptionLength = 20;
        private readonly ITagValidator tagValidator;
        private readonly IBlockBuilder blockBuilder;
        private readonly ITextHelper textHelper;

        private delegate Tag CreateTag(string payload);

        public MarkdownParser(ITagValidator tagValidator, IBlockBuilder blockBuilder, ITextHelper textHelper)
        {
            this.tagValidator = tagValidator;
            this.blockBuilder = blockBuilder;
            this.textHelper = textHelper;
        }
        
        public IBlock Parse()
        {
            var tagInfos = GetTags();
            var validTags = tagValidator.GetValidTags(tagInfos);
            return blockBuilder.Build(validTags);
        }

        /// <summary>
        ///     Parse tags according to documented rules
        /// </summary>
        public IEnumerable<TagInfo> GetTags()
        {
            var processed = 0;
            while (processed < textHelper.GetTextLength())
            {
                var tagInfo = ParseTag(processed);
                var shift = 0;
                if (tagInfo != null)
                {
                    shift = tagInfo.Length;
                    yield return tagInfo;
                }

                processed += Math.Max(shift, 1);
            }

            yield return GetEmptyEnterTag(textHelper.GetTextLength());
        }

        private TagInfo GetEmptyEnterTag(int offset)
        {
            return new TagInfo(offset, 0, Style.NewLine);
        }

        private TagInfo ParseTag(int offset)
        {
            switch (textHelper.GetCharacter(offset))
            {
                case '_':
                    return ParseUnderscore(offset);
                case '\\':
                    return ParseEscapeSymbol(offset);
                case '#':
                    return ParseHeader(offset);
                case '!':
                    return ParsePicture(offset);
                case '[':
                    return ParseLink(offset);
                case ')':
                    return ParseMediaEnd(offset);
                default:
                    return TryParseNewLine(offset, out var tagInfo)
                        ? tagInfo
                        : null;
            }
        }

        private TagInfo ParseMediaEnd(int offset)
        {
            return new TagInfo(offset, 1, Style.Media, true, false);
        }

        private TagInfo ParseLink(int offset)
        {
            Tag CreateTag(string link) { return new LinkTag(link); }
            return ParseMedia(offset, CreateTag, 0);
        }

        private TagInfo ParsePicture(int offset)
        {
            Tag CreateTag(string description) { return new PictureTag(description); }
            return ParseMedia(offset, CreateTag, 1);
        }

        private TagInfo ParseMedia(int offset, CreateTag createTag, int processed)
        {
            var payload = ParsePayload(offset, processed);
            if (payload is null)
                return null;
            processed += payload.Length + 2;

            if (textHelper.CharIs('(', offset + processed))
            {
                processed++;
                return new TagInfo(offset, processed, createTag(payload), false);
            }

            return null;
        }

        private string ParsePayload(int offset, int processed)
        {
            if (textHelper.CharIs('[', offset + processed))
            {
                processed++;
                for (var payloadLength = 0; payloadLength < maxPictureDescriptionLength; payloadLength++)
                    if (textHelper.CharIs(']', offset + payloadLength + processed))
                        return textHelper.Substring(offset + processed, payloadLength);
            }

            return null;
        }

        private TagInfo ParseUnderscore(int offset)
        {
            return textHelper.CharIs('_', offset + 1)
                ? ParseDoubleUnderscore(offset)
                : ParseSingleUnderscore(offset);
        }

        private TagInfo ParseDoubleUnderscore(int offset)
        {
            if (textHelper.CharIs(' ', offset + 1) || offset == textHelper.GetTextLength() - 1)
                return new TagInfo(offset, 2, Style.Bold, true, false);

            if (textHelper.CharIs(' ', offset - 1) || offset == 0)
                return new TagInfo(offset, 2, Style.Bold, false);

            return new TagInfo(offset, 2, Style.Bold);
        }

        private TagInfo ParseSingleUnderscore(int offset)
        {
            if (textHelper.CharIsNumber(offset + 1)
                && !textHelper.CharIsWhiteSpace(offset - 1))
                return null;

            if (textHelper.CharIs(' ', offset + 1) || offset == textHelper.GetTextLength() - 1)
                return new TagInfo(offset, 1, Style.Angled, true, false);

            if (textHelper.CharIs(' ', offset - 1) || offset == 0)
                return new TagInfo(offset, 1, Style.Angled, false);

            return new TagInfo(offset, 1, Style.Angled);
        }

        private TagInfo ParseEscapeSymbol(int offset)
        {
            return textHelper.CharIs('\\', offset + 1)
                ? null
                : new TagInfo(offset, 1, Style.Escape);
        }

        private TagInfo ParseHeader(int offset)
        {
            return textHelper.CharIs(' ', offset + 1)
                ? new TagInfo(offset, 2, Style.Header)
                : null;
        }

        private bool TryParseNewLine(int offset, out TagInfo tagInfo)
        {
            tagInfo = null;
            var newLine = Environment.NewLine;
            if (!textHelper.IsInBounds(offset + newLine.Length))
                return false;
            var substring = textHelper.Substring(offset, newLine.Length);
            if (substring != newLine)
                return false;

            tagInfo = new TagInfo(offset, 2, Style.NewLine);
            return true;
        }
    }
}