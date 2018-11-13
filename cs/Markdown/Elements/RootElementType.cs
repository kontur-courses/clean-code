using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Elements
{
    public class RootElementType : IElementType
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

        public string Indicator => "";

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
