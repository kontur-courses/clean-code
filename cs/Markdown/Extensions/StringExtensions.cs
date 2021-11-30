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

        public static Token GetTokenUntilNewLine(this string text, Token openTag)
        {
            var endPos = text.IndexOf(Environment.NewLine, openTag.Position, StringComparison.Ordinal);
            var value = text.Substring(openTag.Position, endPos == -1
                ? text.Length - openTag.Position
                : endPos - openTag.Position + Environment.NewLine.Length);
            return new Token(value, openTag.Position, openTag.WrapSetting);
        }

        public static Token GetToken(this string text, Token openTag, Token closeTag)
        {
            var value = text.Substring(openTag.Position, closeTag.Position - openTag.Position + closeTag.Value.Length);
            return new Token(value, openTag.Position, openTag.WrapSetting);
        }
    }
}