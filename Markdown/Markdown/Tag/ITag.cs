using System.Collections.Generic;
using Markdown.Attribute;

namespace Markdown.Tag
{
    public interface ITag
    {
        string Symbol { get; }
        int OpenIndex { get; set; }
        int CloseIndex { get; set; }
        string Html { get; }
        int Length { get; }
        string Content { get; set; }
        MdType Type { get; }
        List<MdType> AllowedInnerTypes { get; }
        IAttribute Attribute { get; set; }
        int FindCloseIndex(string text);
        string GetContent(string text);
    }
}