using System.Collections.Generic;
using System.Linq;
using Markdown.Data;

namespace Markdown.TokenParser.TokenReaders
{
    public class EscapedTokenReader : ITokenReader
    {
        private readonly List<string> tags;

        public EscapedTokenReader(IEnumerable<string> tags)
        {
            this.tags = new List<string>(tags.OrderBy(tag => tag.Length)) { "\\" };
        }

        public TokenReaderResult ReadToken(string text, int startingPosition)
        {
            if (startingPosition + 1 >= text.Length || text[startingPosition] != '\\')
                return new TokenReaderResult(false);
            var escapedTag = tags.FirstOrDefault(tag =>
                    startingPosition + tag.Length < text.Length &&
                    tag == text.Substring(startingPosition + 1, tag.Length));
            return escapedTag == null
                ? new TokenReaderResult(false)
                : new TokenReaderResult(true, escapedTag.Length + 1, new Token(TokenType.Text, escapedTag));
        }
    }
}