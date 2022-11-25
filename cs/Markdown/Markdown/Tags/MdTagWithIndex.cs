using System;

namespace Markdown
{
    public class MdTagWithIndex : TagWithIndex
    {
        public MdTagWithIndex(MdTag tag, int openTagIndex, int closeTagIndex)
        {
            OpenTag = tag.OpenTag;
            CloseTag = tag.HasCloseTag ? tag.CloseTag : String.Empty;
            OpenTagIndex = openTagIndex;
            CloseTagIndex = closeTagIndex;
        }
    }
}