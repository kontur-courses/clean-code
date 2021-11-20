using System;

namespace Markdown
{
    public class TokenRulesConfiguratorEnd : TokenParserConfigurator
    {
        private TokenRulesConfiguratorEnd()
        {
        }
        
        public TokenRulesConfigurator And => throw new NotImplementedException();
    }
}