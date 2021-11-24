using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public interface ITokenTranslator
    {
        public string Translate(IEnumerable<IToken> tokens);
    }
}
