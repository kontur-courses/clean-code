using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class HTMLRenderer
    {
        private Dictionary<TokenType, HTMLTag> tokenTagDictionary = new Dictionary<TokenType, HTMLTag>()
        {
            {TokenType.Bold, new HTMLTag("strong") },
            {TokenType.Italic, new HTMLTag("em") },
        };

        //Превращает оставшиеся корректные токены в HTML-теги
        public string Render(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
