using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITokenCreator
    {
        IEnumerable<Token> Create(string text);
    }
}