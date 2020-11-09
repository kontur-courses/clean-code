using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Md
    {
        public static readonly Style HeadingStyle = new Style(StyleType.Heading, "#", "");
        public static readonly Style BoldStyle = new Style(StyleType.Bold, "__", "__");
        public static readonly Style ItalicStyle = new Style(StyleType.Italic, "_", "_");

        public static string Render(string md)
        {
            throw new NotImplementedException();
        }

    }
}
