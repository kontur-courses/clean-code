using System;
using Markdown.ASTNodes;

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
