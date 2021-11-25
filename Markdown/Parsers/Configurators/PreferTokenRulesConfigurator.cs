namespace Markdown
{
    internal class PreferTokenRulesConfigurator : TokenParserConfigurator
    {
        private PreferTokenRulesConfigurator()
        {
        }
        
        protected internal PreferTokenRulesConfigurator(TokenParserConfig config)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(config), config));
            Config = config;
        }
        
        public TokenRulesConfigurator That => new(Config);
    }
}