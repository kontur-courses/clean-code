using System.Collections.Generic;

namespace Markdown
{
    public static class MdTagSymbolDetector
    {
        private static readonly Dictionary<MdType, string> typeTagMapping;
        static MdTagSymbolDetector()
        {
            typeTagMapping = new Dictionary<MdType, string>
            {
                {MdType.SingleUnderLine, "_"},
                {MdType.DoubleUnderLine, "__"},
                {MdType.Sharp, "#"},
                {MdType.TripleGraveAccent, "```"}
            };
        }
        public static string Detect(MdType type) => typeTagMapping[type];
    }
}