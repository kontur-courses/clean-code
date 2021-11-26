namespace Markdown
{
    internal class TokenRulesConfigurator
    {
        private TokenParserConfig config;
        
        private TokenRulesConfigurator()
        {
        }
        
        protected internal TokenRulesConfigurator(TokenParserConfig config)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(config), config));
            this.config = config;
        }
        
        public TokenRulesConfiguratorEnd CanIntersectWith(Tag tag)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tag), tag));
            config.TagRules.SetRule(config.LastAddedToken, tag, InteractType.Intersecting);
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CanBeShielded()
        {
            config.TagRules.SetShieldedTeg(config.LastAddedToken);
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CanBeNestedIn(Tag tag)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tag), tag));
            config.TagRules.SetRule(tag, config.LastAddedToken, InteractType.Nesting);
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CantContain(params char[] symbols)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(symbols), symbols));
            foreach (var symbol in symbols)
            {
                config.TagRules.SetRule(config.LastAddedToken, TagFactory.GetOrAddSingleTag(symbol.ToString()), InteractType.Contain);
                config.Tokens.Add(symbol.ToString());
            }
            
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CanBeInFrontOnly()
        {
            config.TagRules.AddInFrontOnlyTag(config.LastAddedToken);
            return new TokenRulesConfiguratorEnd(config);
        }
    }
}