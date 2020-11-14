using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Models
{
    public class MarkdownElementContext
    {
        public MarkdownElementContext(Token currentToken, IEnumerable<Token> allTokens)
        {
            NextTokens = allTokens.ToArray();
            CurrentToken = currentToken;
        }

        public Token[] NextTokens { get; }
        public Token CurrentToken { get; }
    }
}