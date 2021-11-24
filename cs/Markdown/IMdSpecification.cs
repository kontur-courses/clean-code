using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown
{
    public interface IMdSpecification
    {
        char EscapeSymbol { get; }
        Dictionary<string, Tag> TagByMdRepresentation { get; }
        Dictionary<string, string> EscapeReplaces { get; }
        void CheckMdText(string mdText);
        string PreProcess(string mdText);
    }
}
