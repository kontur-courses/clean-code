using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public abstract class MarkdownElement
    {
        public abstract ParseResult Parse();
    }
}
