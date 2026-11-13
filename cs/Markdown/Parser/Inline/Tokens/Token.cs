using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline.Tokens
{
    public abstract class Token
    {
        public int Position { get; }
        protected Token(int position) => Position = position;
    }
}
