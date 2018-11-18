using System.Linq;

namespace Markdown.Elements
{
    public class RootElementType : EmphasisTypeBase
    {
        private static readonly RootElementType Instance = new RootElementType();
        private static readonly IElementType[] PossibleInnerElementTypes =
             { SingleUnderscoreElementType.Create(), DoubleUnderscoreElementType.Create() };

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

        public override bool IsIndicatorAt(string markdown, bool[] isEscapedCharAt, int position)
        {
            return false;
        }
    }
}
