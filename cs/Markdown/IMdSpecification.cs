using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown
{
    public interface IMdSpecification
    {
        List<Tag> Tags { get; }
        List<string> EscapeSymbols { get; }
        Dictionary<string, Tag> TagByMdStringRepresentation { get; }
        List<string> EscapeSequences { get; }
        Dictionary<string, string> EscapeReplaces { get; }
    }
}
