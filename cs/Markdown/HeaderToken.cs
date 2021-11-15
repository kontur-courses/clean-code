using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal class HeaderToken : Token

    {
        public HeaderToken(int openIndex) : base(openIndex) { }
    }
}
