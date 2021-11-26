using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public class HeaderToken : Token
    {
        public override string OpenedHtmlTag => "<h1>";

        public override string ClosedHtmlTag => "</h1>";

        public override int RawLengthOpen => 2;

        public override int RawLengthClose => 0;

        public HeaderToken(int start, int length) : base(start, length)
        {
        }
    }
}
