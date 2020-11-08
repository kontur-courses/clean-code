using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Models
{
    public class MarkdownElementContext
    {
        public MarkdownElementContext(int currentTokenIndex, IEnumerable<Token> allTokens)
        {
            CurrentTokenIndex = currentTokenIndex;
            AllTokens = allTokens.ToArray();
            CurrentToken = AllTokens[currentTokenIndex];
        }

        public int CurrentTokenIndex { get; }
        public Token[] AllTokens { get; }
        public Token CurrentToken { get; }
    }
}