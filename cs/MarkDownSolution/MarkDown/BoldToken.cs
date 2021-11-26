using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public class BoldToken : Token
    {
        public override string OpenedHtmlTag => "<strong>";

        public override string ClosedHtmlTag => "</strong>";

        public override int RawLengthOpen => 2;

        public override int RawLengthClose => 2;

        public BoldToken(int start) : base(start)
        {
        }

        public BoldToken(int start, int length) : base(start, length)
        {
        }
    }
}
