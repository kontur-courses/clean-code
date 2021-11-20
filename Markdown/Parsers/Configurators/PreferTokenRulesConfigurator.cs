using System;
using System.Collections.Generic;

namespace Markdown
{
    public class PreferTokenRulesConfigurator : TokenParserConfigurator
    {
        protected internal PreferTokenRulesConfigurator(List<Token> tokens)
        {
            Tokens = tokens;
        }
        
        public TokenRulesConfigurator That => throw new NotImplementedException();
    }
}