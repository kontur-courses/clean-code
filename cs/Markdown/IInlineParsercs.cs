using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal interface IInlineParser
    {
        public string Parse(string line);
    }
}
