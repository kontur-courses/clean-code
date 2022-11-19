using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class Title : Tag
    {
        public Title(Md md) : base(md, "#")
        {
        }

        protected override string FormatTag(Token start, Token end, string strBetween)
        {
            //TODO
            return $"<h1>{strBetween}</h1>";
        }

        protected override Token FindEnd(Token start)
        {
            var current = start.Next;
            while (current != null && current.Type != TokenType.BreakLine)
                current = current.Next;
            return current;
        }
    }
}
