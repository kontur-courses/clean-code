using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string markDownParagraph)
        {
            var tokens = GetTokens(markDownParagraph);
            var tags = GetTags(tokens);
            var tokensWithTags = GetTokensMapToTags(tokens, tags);

            return GetHTMLCode(tokensWithTags);
        }

        private IEnumerable<Token> GetTokens(string line)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Tag> GetTags(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Tuple<Tag, string>> GetTokensMapToTags(IEnumerable<Token> tokens, IEnumerable<Tag> tags)
        {
            throw new NotImplementedException();
        }

        private string GetHTMLCode(IEnumerable<Tuple<Tag, string>> tokensMapToTags)
        {
            throw new NotImplementedException();
        }
    }
}