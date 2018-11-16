using System.Linq;

namespace Markdown.Elements
{
    public class DoubleUnderscoreElementType : ElementTypeBase
    {
        private static readonly DoubleUnderscoreElementType Instance = new DoubleUnderscoreElementType();
        private static readonly IElementType[] PossibleInnerElementTypes = 
            { UnderscoreElementType.Create() };

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

        public override bool IsIndicatorAt(string markdown, int position)
        {
            if (position + Indicator.Length > markdown.Length)
                return false;
            if (markdown.Substring(position, Indicator.Length) != Indicator)
                return false;
            int positionAfterIndicator = position + Indicator.Length;
            return positionAfterIndicator >= markdown.Length ||
                   markdown.Substring(positionAfterIndicator, 1) != "_";
        }
    }
}
