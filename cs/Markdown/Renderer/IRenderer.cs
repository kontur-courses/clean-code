using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public interface IRenderer
    {
        public string Render(IEnumerable<Token> tokens, string text);
    }
}