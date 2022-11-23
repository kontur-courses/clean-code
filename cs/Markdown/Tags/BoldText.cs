using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class BoldText : Tag
    {
        public BoldText(Md md) : base(md, "__")
        {
        }

        protected override string FormatTag(Token start, Token end, string contains)
        {
            try
            {
                return base.FormatTag(start, end, contains);
            }
            catch (FormatException)
            {
                if (Markdown.dfsTags.Contains("_"))
                    return NoFormat(start, end, contains);
                return end == null ? $"{start.Value}{contains}" : $"<strong>{contains}</strong>";
            }
        }
    }
}
