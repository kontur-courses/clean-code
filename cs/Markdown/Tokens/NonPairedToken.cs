using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Parser;

namespace Markdown.Tokens
{
    internal abstract class NonPairedToken : Token
    {
        protected NonPairedToken(int openIndex) : base(openIndex) { }
    }
}
