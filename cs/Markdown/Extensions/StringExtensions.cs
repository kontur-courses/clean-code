using System;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveMdTags(this string value, MdWrapSetting setting)
        {
            switch (setting.TagType)
            {
                case MdTagType.Root:
                case MdTagType.Block:
                case MdTagType.Backslash:
                    return value.Remove(0, setting.MdTag.Length);
                case MdTagType.Span:
                    return value.Remove(value.Length - setting.MdTag.Length).Remove(0, setting.MdTag.Length);
                default:
                    throw new NotImplementedException(
                        $"Невозможно выполнить {nameof(RemoveMdTags)} для {setting.TagType}.");
            }
        }

        public static string InsertHtmlTags(this string text, MdWrapSetting setting)
        {
            switch (setting.TagType)
            {
                case MdTagType.Root:
                case MdTagType.Block:
                    return text.Insert(text.EndsWith(Environment.NewLine)
                                ? text.Length - Environment.NewLine.Length
                                : text.Length,
                            setting.HtmlCloseTag)
                        .Insert(0, setting.HtmlOpenTag);
                case MdTagType.Backslash:
                case MdTagType.Span:
                    return text.Insert(text.Length, setting.HtmlCloseTag).Insert(0, setting.HtmlOpenTag);
                default:
                    throw new NotImplementedException(
                        $"Невозможно выполнить {nameof(InsertHtmlTags)} для {setting.TagType}.");
            }
        }

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
    }
}