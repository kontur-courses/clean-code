using System.Collections.Generic;

namespace Markdown
{
    public interface IStringTranslator
    {
        public IEnumerable<Token> Translate(string markdown);
    }
}
