using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tags
{
    public class DoubleUnderscore : Tag
    {
        public DoubleUnderscore(Md md) : base(md, "__")
        {
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            if (Markdown.AmIn("_"))
                return $"{start.Value}{contains}{end.Value}";
            return end == null ? $"{start.Value}{contains}" : $"<strong>{contains}</strong>";
        }
    }
}
