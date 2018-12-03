using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class SimpleElement : IElement
    {
        public IElement Child { get; set; }

        public string Value { get;}

        public SimpleElement(Token token)
        {
            Value = token.Value;
        }
    }
}
