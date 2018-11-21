using System.Collections.Generic;

namespace Markdown.Tag
{
    public class LinkTag : ITag
    {
        public string Symbol => "[";
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string Html => "a";
        public int Length => Symbol.Length;
        public string Content { get; set; }
        public MdType Type => MdType.Link;
        public List<MdType> AllowedInnerTypes =>
            new List<MdType> { MdType.DoubleUnderLine, MdType.SingleUnderLine, MdType.Sharp };
        public int MiddleIndex { get; set; }

        public int FindCloseIndex(string text)
        {
            for (var i = OpenIndex + 2; i < text.Length - 1; i++)
            {
                var twoSymbols = text.Substring(i, Length * 2);
                if (twoSymbols == "](")
                {
                    MiddleIndex = i;
                    break;
                }
            }
            
            if (MiddleIndex == 0)
                return -1;

            for (var i = MiddleIndex + 2; i < text.Length; i++)
                if (text[i].ToString() == ")")
                    return i;

            return -1;
        }

        public string GetContent(string text) => text.Substring(OpenIndex + Length, MiddleIndex - OpenIndex - Length);

        public IAttribute Attribute { get; set; }
    }
}
