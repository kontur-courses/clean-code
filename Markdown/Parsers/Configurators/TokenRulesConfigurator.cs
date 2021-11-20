using System;

namespace Markdown
{
    public class TokenRulesConfigurator
    {
        private TokenRulesConfigurator()
        {
        }
        
        public TokenRulesConfiguratorEnd CantBeNestedIn(Tag tag)
        {
            throw new NotImplementedException();
        }
        
        public TokenRulesConfiguratorEnd CanIntersectWith(Tag tag)
        {
            throw new NotImplementedException();
        }
        
        public TokenRulesConfiguratorEnd CantIntersectWith(Tag tag)
        {
            throw new NotImplementedException();
        }
        
        public TokenRulesConfiguratorEnd CanBeNestedIn(Tag tag)
        {
            throw new NotImplementedException();
        }
        
        public TokenRulesConfiguratorEnd CantIntersect()
        {
            throw new NotImplementedException();
        }
        
        public TokenRulesConfiguratorEnd CantBeNested()
        {
            throw new NotImplementedException();
        }
        
        public TokenRulesConfiguratorEnd CanBeNestedInAnyTokens()
        {
            throw new NotImplementedException();
        }
        
        public TokenRulesConfiguratorEnd CanIntersectWithAnyTokens()
        {
            throw new NotImplementedException();
        }
    }
}