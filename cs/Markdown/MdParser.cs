using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdParser
    {
        private readonly string Text;

        public MdParser(string text)
        {
            Text = text;
        }
        
        public IEnumerable<Token> GetTokens()
        {
            throw new NotImplementedException();
        }
    }
}