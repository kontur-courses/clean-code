using System.Collections.Generic;

namespace Markdown
{
    public interface IConverter
    {
        string ReplaceAttributesWithTags(IEnumerable<Token> tokens, string source);
    }
}   