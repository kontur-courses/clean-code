using System;

namespace Markdown
{
    public class TokenTranslatorConfigurator
    {
        public static TokenTranslatorConfigurator CreateTokenTranslator()
        {
            throw new NotImplementedException();
        }
        
        public TokenTranslatorConfigurator SetReference(Token first, Token second)
        {
            throw new NotImplementedException();
        }
    }
    
    public interface ITokenTranslator
    {
        void SetTranslateRule(Token from, Token to);
        Token Translate(Token token);
    }
}