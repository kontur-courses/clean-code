using System;
using System.Linq;

namespace Markdown.Elements
{
    public class DoubleUnderscoreElementType : UnderscoreElementTypeBase
    {
        private static readonly DoubleUnderscoreElementType Instance = new DoubleUnderscoreElementType();
        private static readonly IElementType[] PossibleInnerElementTypes = 
            { SingleUnderscoreElementType.Create() };

        private DoubleUnderscoreElementType()
        { }

        public static DoubleUnderscoreElementType Create()
        {
            return Instance;
        }

        public override string Indicator => "__";

        public override bool CanContainElement(IElementType elementType)
        {
            return PossibleInnerElementTypes.Contains(elementType);
        }
    }
}
