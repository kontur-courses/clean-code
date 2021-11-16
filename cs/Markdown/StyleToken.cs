using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal abstract class StyleToken : Token
    {
        protected StyleToken(int openIndex) : base(openIndex) { }

        protected bool IsTokenInsideWord()
        {
            throw new NotImplementedException();
        }

        protected bool IsTokenInsideDifferentWords()
        {
            throw new NotImplementedException();
        }
    }
}
