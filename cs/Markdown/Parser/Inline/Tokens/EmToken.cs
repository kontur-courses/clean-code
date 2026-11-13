using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.Tokens
{
    public class EmToken : PairToken
    {
        public EmToken(int position) : base(position) { }
    }
}
