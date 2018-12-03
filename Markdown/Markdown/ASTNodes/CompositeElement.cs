using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class CompositeElement : IElement
    {
        
        public IElement LeftChild { get; set; }
        public IElement Child { get; set; }

        public Token[] Tokens { get; }

        public CompositeElement(Token[] tokens)
        {
            Tokens = tokens;
        }
    }
}
