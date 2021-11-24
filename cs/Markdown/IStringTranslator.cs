using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public interface IStringTranslator
    {
        public IEnumerable<IToken> Translate(string markdown);
    }
}
