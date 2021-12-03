using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public interface IRenderer<TToken>
    {
        public string Render(IEnumerable<TToken> tokens);
    }
}