using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenTranslator
    {
        public string Translate(IEnumerable<Token> tokens);
    }
}
