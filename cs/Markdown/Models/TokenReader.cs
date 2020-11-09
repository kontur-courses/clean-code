using System;
using System.Collections.Generic;
using Markdown.Models.Rules;
using Markdown.Models.Tags;

namespace Markdown.Models
{
    internal class TokenReader
    {
        private readonly IEnumerable<Tag> tags;

        public TokenReader(IEnumerable<Tag> tags)
        {
            this.tags = tags;
        }

        public IEnumerable<TaggedToken> ReadTokens(string text)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<TaggedToken> GetRawTokens(string text)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<TaggedToken> RemoveEscapeSymbols(IEnumerable<TaggedToken> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
