using System;
using System.Collections.Generic;

namespace Markdown.Tags
{
    public class ItalicText : Tag
    {
        public ItalicText(Md md) : base(md, "_", new HashSet<char>())
        {
        }

        protected override string FormatTag(Token start, Token end, string strBetween)
        {
            try
            {
                var result = base.FormatTag(start, end, strBetween);
                return result;
            }
            catch (FormatException)
            {
                return end == null ? $"{start.Value}{strBetween}" : $"<em>{strBetween}</em>";
            }
        }
    }
}