using System.Collections.Generic;

namespace Markdown
{
    public interface IMdSpecification
    {
        List<Tag> Tags { get; }
        List<string> EscapeSymbols { get; }
        Dictionary<string, Tag> TagByMdStringRepresentation { get; }
        List<string> EscapeSequences { get; }
    }
}
