using System.Collections.Generic;

namespace Markdown
{
    public interface IConverter
    {
        string ReplaceAttributesWithTags(IEnumerable<IToken> tokens, string source);
    }
}   