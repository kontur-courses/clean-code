using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class ItalicText : Tag
    {
        public ItalicText(Md md) : base(md, "_")
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
