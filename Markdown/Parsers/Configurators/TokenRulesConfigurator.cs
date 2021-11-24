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
            this.config = config;
        }
        
        public TokenRulesConfiguratorEnd CanIntersectWith(Tag tag)
        {
            config.TagRules.SetRule(config.LastAddedToken, tag, InteractType.Intersecting);
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CanBeNestedIn(Tag tag)
        {
            config.TagRules.SetRule(tag, config.LastAddedToken, InteractType.Nesting);
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CantContain(params char[] symbols)
        {
            foreach (var symbol in symbols)
            {
                config.TagRules.SetRule(config.LastAddedToken, Tag.GetOrAddSingleTag(symbol.ToString()), InteractType.Contain);
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