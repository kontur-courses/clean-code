using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITokenCreator
    {
        IEnumerable<IToken> Create(string text);
    }
}