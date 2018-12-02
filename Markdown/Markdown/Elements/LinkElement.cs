using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Elements
{
    class LinkElement : IElement
    {
        public IElement Child { get; set; }

        public LinkElement(IElement child)
        {
            Child = child;
        }
    }
}