using System.Linq;

namespace Markdown.Elements
{
    public class RootElementType : ElementTypeBase
    {
        private static readonly RootElementType Instance = new RootElementType();
        private static readonly IElementType[] PossibleInnerElementTypes =
             { UnderscoreElementType.Create(), DoubleUnderscoreElementType.Create() };

        private RootElementType()
        { }

        public static RootElementType Create()
        {
            return Instance;
        }

        public override string Indicator => "";

        public override bool CanContainElement(IElementType elementType)
        {
            return PossibleInnerElementTypes.Contains(elementType);
        }

        public override bool IsIndicatorAt(string markdown, int position)
        {
            return false;
        }
    }
}
