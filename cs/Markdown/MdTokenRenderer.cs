using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class MdTokenRenderer
    {
        public string Text;

        protected MdTokenRenderer(string text)
        {
            Text = text;
        }

        public abstract string Render(MdToken token);

        public string RenderAll(IEnumerable<MdToken> tokens) => string.Concat(tokens.Select(Render));

        protected string RenderSubtokens(MdTokenWithSubTokens token) => string.Concat(token.SubTokens.Select(Render));
    }
}