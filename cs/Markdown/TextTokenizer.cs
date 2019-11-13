using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TextTokenizer
    {
        private string[] specialSymbols;
        private Dictionary<string, TokenType> symbolTypes;

        public TextTokenizer(string[] specialSymbols, Dictionary<string, TokenType> symbolTypes)
        {
            this.specialSymbols = specialSymbols;
            this.symbolTypes = symbolTypes;
        }

        public IEnumerable<Token> GetTokens(string rawText)
        {
            throw new NotImplementedException();
        }
    }
}

