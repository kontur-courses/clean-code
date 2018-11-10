using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    abstract class MarkdownElementBase : IMarkdownElement
    {
        public int StartPosition { get; protected set; }
        public int EndPosition { get; protected set; }
        public IReadOnlyList<IMarkdownElement> InnerElements { get; protected set; }
        public string Indicator { get; protected set; }
        protected Type[] PossibleInnerElementTypes;

        protected MarkdownElementBase(int start, int end, IReadOnlyList<IMarkdownElement> elements, 
            string indicator, Type[] possibleInnerElementTypes)
        {
            StartPosition = start;
            EndPosition = end;
            InnerElements = elements;
            Indicator = indicator;
            PossibleInnerElementTypes = possibleInnerElementTypes;
        }

        public bool CanContainElement(IMarkdownElement element)
        {
            return PossibleInnerElementTypes.Contains(element.GetType());
        }
    }
}
