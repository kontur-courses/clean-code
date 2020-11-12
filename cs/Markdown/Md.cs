using System.Collections.Generic;

namespace Markdown
{
    internal class Md
    {
        public static readonly Style HeadingStyle = new Style(StyleType.Heading, "#", "");
        public static readonly Style BoldStyle = new Style(StyleType.Bold, "__", "__");
        public static readonly Style ItalicStyle = new Style(StyleType.Italic, "_", "_");

        public static readonly Dictionary<StyleType, Style> mdStyles = new Dictionary<StyleType, Style>
        {
            [StyleType.Heading] = HeadingStyle,
            [StyleType.Bold] = BoldStyle,
            [StyleType.Italic] = ItalicStyle
        };

        public static string Render(string md)
        {
            return MdToHTMLConverter.Convert(md);
        }
    }
}