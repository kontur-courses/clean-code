using Markdown.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDElements
{
    public class ItalicNode : PairedNode
    {
        public ItalicNode(string opening, string closing)
            : base(opening, closing) {}
    }
}
