using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface IParser
    {
        IEnumerable<IToken> Parse(string expression);
    }
}