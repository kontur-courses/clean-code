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
            return false;
        }
    }
}
