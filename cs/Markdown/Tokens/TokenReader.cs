using System;
using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public class TokenReader
    {
        private readonly TagStorage tagStorage;

        public TokenReader(TagStorage tagStorage)
        {
            this.tagStorage = tagStorage;
        }
        public IReadOnlyList<Token> Read(string inputText)
        {
            throw new NotImplementedException();
        }
    }
}
