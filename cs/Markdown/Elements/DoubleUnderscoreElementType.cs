using System.Linq;

namespace Markdown.Elements
{
    public class DoubleUnderscoreElementType : IElementType
    {
        private static readonly DoubleUnderscoreElementType Instance = new DoubleUnderscoreElementType();
        private static readonly IElementType[] PossibleInnerElementTypes = 
            new[] { UnderscoreElementType.Create() };

        private DoubleUnderscoreElementType()
        { }

        public static DoubleUnderscoreElementType Create()
        {
            return Instance;
        }

        public string Indicator => "__";

        public bool CanContainElement(IElementType elementType)
        {
            return PossibleInnerElementTypes.Contains(elementType);
        }

        public bool IsIndicatorAt(string markdown, int position)
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
