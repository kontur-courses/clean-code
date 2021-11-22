namespace Markdown
{
    internal class PreferTokenRulesConfigurator : TokenParserConfigurator
    {
        private PreferTokenRulesConfigurator()
        {
        }
        
        protected internal PreferTokenRulesConfigurator(TokenParserConfig config)
        {
            Config = config;
        }
        
        public TokenRulesConfigurator That => new(Config);
    }
}