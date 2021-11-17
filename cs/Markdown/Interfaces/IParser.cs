using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface IParser
    {
        IEnumerable<TagToken> Parse(string expression);
    } 
}
