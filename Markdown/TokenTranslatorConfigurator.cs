using System;

namespace Markdown
{
    public class TokenTranslatorToConfigurator
    {
        public TokenTranslatorConfigurator To(Token token)
        {
            throw new NotImplementedException();
        }
    } 
    
    public class TokenTranslatorFromConfigurator
    {
        public TokenTranslatorToConfigurator From(Token token)
        {
            throw new NotImplementedException();
        }
    }
    
    public class TokenTranslatorConfigurator
    {
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