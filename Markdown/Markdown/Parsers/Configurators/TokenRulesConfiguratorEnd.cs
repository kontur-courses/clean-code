namespace Markdown
{
    internal class TokenRulesConfiguratorEnd : TokenParserConfigurator
    {
        protected internal TokenRulesConfiguratorEnd(TokenParserConfig config)
        {
            Config = config;
        }

        public TokenRulesConfigurator And => new(Config);
    }
}