using System;

namespace Markdown
{
    internal class TokenRulesConfigurator
    {
        private readonly TokenParserConfig config;
        
        protected internal TokenRulesConfigurator(TokenParserConfig config)
        {
            this.config = config;
        }

        public TokenRulesConfiguratorEnd CanBeShielded()
        {
            if (config.LastAddedToken is null) throw new ArgumentNullException(nameof(config.LastAddedToken));
            config.TagRules.SetShieldedTeg(config.LastAddedToken);
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CanBeNestedIn(Tag tag)
        {
            if (config.LastAddedToken is null) throw new ArgumentNullException(nameof(config.LastAddedToken));
            config.TagRules.SetRule(tag, config.LastAddedToken, InteractType.Nesting);
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CantContain(params char[] symbols)
        {
            if (config.LastAddedToken is null) throw new ArgumentNullException(nameof(config.LastAddedToken));
            foreach (var symbol in symbols)
            {
                config.TagRules.SetRule(config.LastAddedToken, TagFactory.GetOrAddSingleTag(symbol.ToString()), InteractType.Contain);
                config.Tokens.Add(symbol.ToString());
            }
            
            return new TokenRulesConfiguratorEnd(config);
        }

        public TokenRulesConfiguratorEnd CanBeInFrontOnly()
        {
            if (config.LastAddedToken is null) throw new ArgumentNullException(nameof(config.LastAddedToken));
            config.TagRules.AddInFrontOnlyTag(config.LastAddedToken);
            return new TokenRulesConfiguratorEnd(config);
        }
    }
}