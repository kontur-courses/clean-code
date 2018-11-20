using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Analyzers;

namespace Markdown.Elements
{
    public class BrokenElementType : EmphasisTypeBase
    {
        private static readonly BrokenElementType Instance = new BrokenElementType();

        private BrokenElementType()
        { }

        public static BrokenElementType Create()
        {
            return Instance;
        }

        public override string Indicator => "";

        public override bool CanContainElement(IElementType elementType)
        {
            return true;
        }

        public override bool IsIndicatorAt(SyntaxAnalysisResult syntaxAnalysisResult, int position)
        {
            return false;
        }
    }
}
