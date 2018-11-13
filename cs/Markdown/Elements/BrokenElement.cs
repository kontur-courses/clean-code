using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Elements
{
    public class BrokenElementType : IElementType
    {
        private static readonly BrokenElementType Instance = new BrokenElementType();

        private BrokenElementType()
        { }

        public static BrokenElementType Create()
        {
            return Instance;
        }

        public string Indicator => "";

        public bool CanContainElement(IElementType elementType)
        {
            return true;
        }

        public bool IsIndicatorAt(string markdown, int position)
        {
            return false;
        }
    }
}
