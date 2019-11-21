using System.Collections.Generic;

namespace Markdown
{
    public interface IConverter
    {
        string ReplaceAttributesWithTags(TokenText tokenText);
    }
}   