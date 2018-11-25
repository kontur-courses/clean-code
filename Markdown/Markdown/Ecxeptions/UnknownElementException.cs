using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Ecxeptions
{
    public class UnknownElementException : Exception
    {
        private IElement element;

        public UnknownElementException(IElement unknownElement)
        {
            element = unknownElement;
        }

        public override string Message => $"Unknown element {element}.";

    }
}
