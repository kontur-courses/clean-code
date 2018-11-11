using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class HtmlBuilder
    {
        private readonly Dictionary<TokenType, string> markupReplacement;

        public HtmlBuilder(Dictionary<TokenType, string> markupReplacement)
        {
            this.markupReplacement = markupReplacement;
        }

        public string Build(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
