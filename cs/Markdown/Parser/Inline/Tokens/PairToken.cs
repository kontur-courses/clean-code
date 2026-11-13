using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.Tokens
{
    public abstract class PairToken : Token
    {
        public bool CanOpen { get; set; }
        public bool CanClose { get; set; }
        public bool IsInsideWord { get; set; }
        public bool IsDisabledFromIntersection { get; set; }

        protected PairToken(int position) : base(position) { }
    }
}
