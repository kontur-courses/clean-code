using System;

namespace Markdown
{
    internal class TokenRulesConfiguratorEnd : TokenParserConfigurator
    {
        private TokenRulesConfiguratorEnd()
        {
        }
        
        public TokenRulesConfigurator And => throw new NotImplementedException();
    }
}