using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITokenRenderer
    {
        string Render(IEnumerable<TokenNode> tokens);
    }
}