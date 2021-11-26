namespace Markdown
{
    internal class PreferTokenRulesConfigurator : TokenParserConfigurator
    {
        protected internal PreferTokenRulesConfigurator(TokenParserConfig config)
        {
            Config = config;
        }
        
        public TokenRulesConfigurator That => new(Config);
    }
}