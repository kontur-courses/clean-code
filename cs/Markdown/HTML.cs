using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class HTML
    {
        public static readonly Style HeadingStyle = new Style(StyleType.Heading, "<h1>", "</h1>");
        public static readonly Style BoldStyle = new Style(StyleType.Bold, "<strong>", "</strong>");
        public static readonly Style ItalicStyle = new Style(StyleType.Italic, "<em>", "</em>");
    }
}
