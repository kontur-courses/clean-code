using System.Collections.Generic;

namespace Markdown.Tag
{
    public class TextTag : ITag
    {
        public string Symbol { get; }
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string Html { get; }
        public int Length { get; }
        public string Content { get; set; }
        public MdType Type => MdType.Text;
        public List<MdType> AllowedInnerTypes { get; }
    }
}
