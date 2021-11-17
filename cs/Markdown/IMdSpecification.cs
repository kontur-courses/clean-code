using System.Collections.Generic;

namespace Markdown
{
    public interface IMdSpecification
    {
        Dictionary<string, string> MdToHTML { get; }
        Dictionary<string, string> HTMLToMd { get; }
        List<string> MdTags { get; }
        List<string> EscapeSymbols { get; }
    }
}
