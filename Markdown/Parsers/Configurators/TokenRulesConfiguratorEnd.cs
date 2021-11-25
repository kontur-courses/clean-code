namespace Markdown
{
    internal class TokenRulesConfiguratorEnd : TokenParserConfigurator
    {
        private TokenRulesConfiguratorEnd()
        {
        }
        
        protected internal TokenRulesConfiguratorEnd(TokenParserConfig config)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(config), config));
            Config = config;
        }

        public TokenRulesConfigurator And => new(Config);
    }
}