using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Models
{
    public class MarkdownElementContext
    {
        public MarkdownElementContext(Token currentToken, IEnumerable<Token> allTokens)
        {
            Tokens = allTokens.ToArray();
            CurrentToken = currentToken;
        }

        public Token[] Tokens { get; }
        public Token CurrentToken { get; }
    }
}