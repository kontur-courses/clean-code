using System;
using System.Collections.Generic;

namespace MarkdownProcessor
{
    public class Tokenizer
    {
        private HashSet<string> tags;
        

        public Tokenizer(HashSet<string> tags)
        {
            this.tags = tags;
        }

        public IEnumerable<Token> GetTokens(string input)
        {
            throw new NotImplementedException();
        }
    }
}