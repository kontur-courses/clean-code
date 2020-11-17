using System.Collections.Generic;
using Markdown.Finders;

namespace Markdown
{
    public class Md
    {
        public static readonly Style HeadingStyle = new Style(StyleType.Heading, "#", "");
        public static readonly Style BoldStyle = new Style(StyleType.Bold, "__", "__");
        public static readonly Style ItalicStyle = new Style(StyleType.Italic, "_", "_");
        public static readonly Style UnorderedListStyle = new Style(StyleType.UnorderedList, "", "");
        public static readonly Style UnorderedListElementStyle = new Style(StyleType.UnorderedListElement, "+ ", "");

        public static readonly HashSet<char> MarkupChars = new HashSet<char>
        {
            '_',
            '#',
            '+'
        };

        public static string Render(string md)
        {
            var converter = new MdToHTMLConverter();
            var textInfo = new EscapingFinder(md, MarkupChars).Find();
            var comparer = new TokenComparer();
            return converter.UsingComparer(comparer).Convert(md, GetMdTokensFinder(md, textInfo).Find(), textInfo);
        }

        private static ITokensFinder GetMdTokensFinder(string md, TextInfo textInfo)
        {
            var headingFinder = new MdHeadingStyleFinder(HeadingStyle, textInfo);
            var boldFinder = new MdBoldStyleFinder(BoldStyle, textInfo);
            var italicFinder = new MdItalicStyleFinder(ItalicStyle, textInfo, BoldStyle, boldFinder);
            var ulElementsFinder = new MdUnorderedListElementStyleFinder(UnorderedListElementStyle, textInfo);
            var ulFinder = new UnorderedListStyleFinder(UnorderedListStyle, textInfo, ulElementsFinder);
            return new MdTokensFinder(md)
                .Using(HeadingStyle, headingFinder)
                .Using(BoldStyle, boldFinder)
                .Using(ItalicStyle, italicFinder)
                .Using(UnorderedListElementStyle, ulElementsFinder)
                .Using(UnorderedListStyle, ulFinder)
                .ExcludingContaining(ItalicStyle, BoldStyle)
                .ExcludingIntersection(ItalicStyle, BoldStyle);
        }
    }
}