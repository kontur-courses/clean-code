using System;
using System.Collections.Generic;
using Markdown.Models.Syntax;
using Markdown.Models.Tags;

namespace Markdown.Models
{
    internal class TokenReader
    {
        private readonly ISyntax syntax;

        public TokenReader(ISyntax syntax)
        {
            this.syntax = syntax;
        }

        public IEnumerable<TaggedToken> ReadTokens(string text)
        {
            var tags = GetRawTags(text);
            tags = RemoveEscapedTags(tags);
            tags = RemoveNonValidTags(tags);
            tags = ValidateTagPairs(tags);
            return GetTokensFromTags(tags);
        }

        private IEnumerable<Tag> GetRawTags(string text)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Tag> RemoveEscapedTags(IEnumerable<Tag> tags)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Tag> RemoveNonValidTags(IEnumerable<Tag> tags)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Tag> ValidateTagPairs(IEnumerable<Tag> tags)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<TaggedToken> GetTokensFromTags(IEnumerable<Tag> tags)
        {
            throw new NotImplementedException();
        }

        private TaggedToken CreateToken(Tag tag, string text)
        {
            throw new NotImplementedException();
        }

        private bool IsValidTag(Tag tag, string text)
        {
            throw new NotImplementedException();
        }
    }
}
