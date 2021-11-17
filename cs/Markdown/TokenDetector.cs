using System;
using System.Collections.Generic;
using Markdown.Tags;
using Markdown.TagStore;

namespace Markdown
{
    public class TokenDetector : ITokenDetector
    {
        private string[] tags;

        public TokenDetector(ITagStore store)
        {
            this.tags = tags;
        }

        public Token GetNextToken(int from, string text)
        {
            throw new NotImplementedException();
        }
    }
}