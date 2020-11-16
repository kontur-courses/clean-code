using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public static readonly Style HeadingStyle = new Style(StyleType.Heading, "#", "");
        public static readonly Style BoldStyle = new Style(StyleType.Bold, "__", "__");
        public static readonly Style ItalicStyle = new Style(StyleType.Italic, "_", "_");

        public static readonly HashSet<char> MarkupChars = new HashSet<char>
        {
            '_',
            '#'
        };

        public static string Render(string md)
        {
            var converter = new MdToHTMLConverter();
            var textInfo = new EscapingFinder(md, MarkupChars).Find();
            return converter.Convert(md, GetMdTokensFinder(md, textInfo).Find(), textInfo);
        }

        private static ITokensFinder GetMdTokensFinder(string md, TextInfo textInfo)
        {
            var headingFinder = new MdHeadingStyleFinder(HeadingStyle, textInfo);
            var boldFinder = new MdBoldStyleFinder(BoldStyle, textInfo);
            var italicFinder = new MdItalicStyleFinder(ItalicStyle, textInfo, BoldStyle, boldFinder);
            return new MdTokensFinder(md)
                .Using(HeadingStyle, headingFinder)
                .Using(BoldStyle, boldFinder)
                .Using(ItalicStyle, italicFinder)
                .ExcludingContaining(ItalicStyle, BoldStyle)
                .ExcludingIntersection(ItalicStyle, BoldStyle);
        }
    }
}