using Markdown.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TokenReader
    {
        private readonly Dictionary<char, ITokenParser> tokenParsers;

        public TokenReader()
        {
            tokenParsers = new Dictionary<char, ITokenParser>();
            InitParsers();
        }

        private void InitParsers()
        {
            tokenParsers['_'] = new ItalicParser();
            // .....
        }

        public IEnumerable<Token> ReadTokens(string text)
        {
            throw new NotImplementedException();
        }
    }
}
