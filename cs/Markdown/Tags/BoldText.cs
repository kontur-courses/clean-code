using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class BoldText : Tag
    {
        public BoldText(Md md) : base(md, "__", new HashSet<char>())
        {
        }

        protected override string FormatTag(Token start, Token end, string strBetween)
        {
            try
            {
                return base.FormatTag(start, end, strBetween);
            }
            catch (FormatException)
            {
                if (Markdown.dfsTags.Contains("_"))
                    return NoFormat(start, end, strBetween);
                return end == null ? $"{start.Value}{strBetween}" : $"<strong>{strBetween}</strong>";
            }
        }
    }
}
