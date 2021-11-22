namespace Markdown
{
    internal class TokenRulesConfiguratorEnd : TokenParserConfigurator
    {
        private TokenRulesConfiguratorEnd()
        {
        }
        
        protected internal TokenRulesConfiguratorEnd(TokenParserConfig config)
        {
            Config = config;
        }

        public TokenRulesConfigurator And => new(Config);
    }
}