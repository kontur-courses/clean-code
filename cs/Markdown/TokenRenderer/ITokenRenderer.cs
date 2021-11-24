using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.TokenRenderer
{
    public interface ITokenRenderer
    {
        string Render(IEnumerable<TagNode> tokens);
    }
}