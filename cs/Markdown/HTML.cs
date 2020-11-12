﻿using System.Collections.Generic;

namespace Markdown
{
    internal class HTML
    {
        public static readonly Style HeadingStyle = new Style(StyleType.Heading, "<h1>", "</h1>");
        public static readonly Style BoldStyle = new Style(StyleType.Bold, "<strong>", "</strong>");
        public static readonly Style ItalicStyle = new Style(StyleType.Italic, "<em>", "</em>");

        public static readonly Dictionary<StyleType, Style> htmlStyles = new Dictionary<StyleType, Style>
        {
            [StyleType.Heading] = HeadingStyle,
            [StyleType.Bold] = BoldStyle,
            [StyleType.Italic] = ItalicStyle
        };
    }
}