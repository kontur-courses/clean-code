using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public enum Style { Italic, Bold, Code }

    public static class StyleExtentions
    {
        public static string MdTag(this Style me)
        {
            switch (me)
            {
                case Style.Italic:
                    return "_";
                case Style.Bold:
                    return "__";
                case Style.Code:
                    return "`";
                default:
                    throw new NotImplementedException($"The Tag of style \"{me.ToString()}\" is not defined.");
            }
        }

        public static string HtmlTag(this Style me)
        {
            switch (me)
            {
                case Style.Italic:
                    return "em";
                case Style.Bold:
                    return "strong";
                case Style.Code:
                    return "code";
                default:
                    throw new NotImplementedException($"The Html Tag of style \"{me.ToString()}\" is not defined.");
            }
        }

        public static string OpenHtmlTag(this string tag) => $"<{tag}>";

        public static string CloseHtmlTag(this string tag) => $"</{tag}>";

        private static Style[] ValidOuterStyles(this Style me)
        {
            switch (me)
            {
                case Style.Italic:
                    return new Style[] { Style.Bold };
                default:
                    return null;
            }
        }

        public static bool IsValidInside(this Style me, Style outerStyle) =>
            me.ValidOuterStyles().Any(vos => vos == outerStyle);

        public static bool IsValidInside(this Style me, IEnumerable<Style> outerStyles) =>
            outerStyles.All(os => me.IsValidInside(os));

        public static bool IsTag(this Style me, ref string text, int index) =>
            text.TryGetSubstring(index, me.MdTag().Length, out string substr) && substr.Equals(me.MdTag());

        public static bool CanBegin(this Style me, ref string text, int index, Stack<(Style style, int endIndex)> outerStyles, out int endIndex)
        {
            if (me.HasNonSpaceSymbolAfterTag(ref text, index)
                && me.IsValidInside(outerStyles.Select(kv => kv.style))
                && !text.IsInsideWordWithNumbers(index))
            {
                var endOfSearchingIndex = outerStyles.Count > 0 ? outerStyles.Peek().endIndex - me.MdTag().Length : text.Length - 1;
                int i = index + me.MdTag().Length + 1;
                while (i <= endOfSearchingIndex)
                {
                    if (me.CanEnd(ref text, i))
                    {
                        endIndex = i;
                        return true;
                    }
                    i++;
                }
            }

            endIndex = default;
            return false;
        }

        public static bool CanEnd(this Style me, ref string text, int index) =>
            me.HasNonSpaceSymbolBeforeTag(ref text, index) && !text.IsInsideWordWithNumbers(index);

        public static bool HasNonSpaceSymbolBeforeTag(this Style _, ref string text, int index)
        {
            var indBefore = index - 1;
            return indBefore >= 0 && text[indBefore] != ' ' && text[indBefore] != '\\';
        }

        public static bool HasNonSpaceSymbolAfterTag(this Style me, ref string text, int index)
        {
            var indAfter = index + me.MdTag().Length;
            return indAfter < text.Length && text[indAfter] != ' ';
        }
    }
}
