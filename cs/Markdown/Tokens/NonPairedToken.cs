using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Parser;

namespace Markdown.Tokens
{
    public abstract class NonPairedToken : Token
    {
        protected NonPairedToken(int openIndex) : base(openIndex) { }
        protected NonPairedToken(int openIndex, int closeIndex) : base(openIndex, closeIndex) { }
    }
}
