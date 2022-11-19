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
            //TODO
            return $"<strong>{contains}</strong>";
        }
    }
}
