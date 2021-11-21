using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IMdParser
    {
        public void AddPossibleToken(string separator, Func<int, Token> tokenInstanceCreator);
        public IEnumerable<Token> ParseTokens(string textToParse);
    }
}