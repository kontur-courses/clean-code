using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Parser
{
    public interface IMdParser
    {
        public IEnumerable<Token> ParseTokens(string textToParse);
    }
}