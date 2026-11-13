using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.Tokens
{
    public class StrongToken : PairToken
    {
        public StrongToken(int position) : base(position) { }
    }
}
