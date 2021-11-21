using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class EscapeToken : Token
    {
        public EscapeToken(int begin, int end) : base(begin, end)
        {
        }

        public override bool AllowInners => false;
        public override int RenderDelta => -1;
        public override string Render(string str, int offset = 0)
        {
            return str.Substring(Begin + 1, Length - 1);
        }
    }
}
