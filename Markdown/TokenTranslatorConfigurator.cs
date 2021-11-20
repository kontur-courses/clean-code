using System;

namespace Markdown
{
    public class TokenTranslatorToConfigurator
    {
        private TokenTranslatorToConfigurator(){}
        
        public TokenTranslatorConfigurator To(Tag tag)
        {
            throw new NotImplementedException();
        }
    } 
    
    public class TokenTranslatorFromConfigurator
    {
        private TokenTranslatorFromConfigurator(){}
        
        public TokenTranslatorToConfigurator From(Tag tag)
        {
            throw new NotImplementedException();
        }
    }
    
    public class TokenTranslatorConfigurator
    {
        protected TokenTranslatorConfigurator(){}
        
        public static TokenTranslatorConfigurator CreateTokenTranslator()
        {
            throw new NotImplementedException();
        }
        
        public TokenTranslatorFromConfigurator SetReference()
        {
            throw new NotImplementedException();
        }

        public ITokenTranslator Configure()
        {
            throw new NotImplementedException();
        }
    }
}