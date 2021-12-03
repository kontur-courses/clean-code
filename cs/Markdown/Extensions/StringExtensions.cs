using System;
using Markdown.Common;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static bool IsSubstring(this string text, int pos, string value, bool isForward = true)
        {
            if (isForward ? pos + value.Length > text.Length : pos - value.Length < 0)
                return false;

            var substring = isForward
                ? text.Substring(pos, value.Length)
                : text.Substring(pos - value.Length, value.Length);

            return substring == value;
        }

        public static bool? IsSubstring(this string text, int pos, Predicate<char> predicate, bool isForward = true)
        {
            if (isForward ? pos + 1 > text.Length : pos - 1 < 0)
                return null;

            pos = isForward ? pos : pos - 1;
            return predicate.Invoke(text[pos]);
        }

        public static int GetEndOfLine(this string text, int startIndex = 0)
        {
            var newLinePos = text.IndexOf(Environment.NewLine, startIndex, StringComparison.Ordinal);
            return newLinePos != -1 ? newLinePos + Environment.NewLine.Length : text.Length;
        }

        // public static Token GetTokenUntilNewLine(this string text, Tag openTag)
        // {
        //     var endPos = text.IndexOf(Environment.NewLine, openTag.Position, StringComparison.Ordinal);
        //     var value = text.Substring(openTag.Position, endPos == -1
        //         ? text.Length - openTag.Position
        //         : endPos - openTag.Position + Environment.NewLine.Length);
        //     return new Token(value, openTag.Position, openTag.MdTagType);
        // }

        // public static Token GetToken(this string text, Tag openTag, Tag closeTag)
        // {
        //     var value = text.Substring(openTag.Position, closeTag.Position - openTag.Position + closeTag.MdTagType.MdTag.Length);
        //     return new Token(value, openTag.Position, openTag.MdTagType);
        // }
        
        public static Token GetToken(this string text, int startIndex, int stopIndex, BaseMdTag tag)
        {
            var value = text.Substring(startIndex, stopIndex - startIndex);
            return new Token(value, startIndex, tag);
        }
    }
}