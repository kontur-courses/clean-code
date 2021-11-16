using System.Collections.Generic;

namespace Markdown
{
    public interface IRenderer
    {
        public string Render(IEnumerable<Token> tokens, string text);
    }
}
