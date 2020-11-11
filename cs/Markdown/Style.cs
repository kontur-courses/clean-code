using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;

namespace Markdown
{
    public class Style
    {
        public readonly StyleType Type;
        public readonly string StartTag;
        public readonly string EndTag;

        public Style (StyleType type, string startTag, string endTag)
        {
            Type = type;
            StartTag = startTag;
            EndTag = endTag;
        }
    }

    public enum StyleType
    {
        Heading,
        Italic,
        Bold
    }
}
