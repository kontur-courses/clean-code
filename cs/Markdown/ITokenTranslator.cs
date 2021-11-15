using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public interface ITokenTranslator
    {
        public string Translate(IEnumerable<Token> tokens);
    }
}
